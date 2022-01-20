using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace Workers;

public abstract class BaseSqsWorker<TMessageType>: BackgroundService
{
    private readonly ILogger<BaseSqsWorker<TMessageType>> _logger;
    private readonly IAmazonSQS _amazonSqs;
    
    protected BaseSqsWorker(ILogger<BaseSqsWorker<TMessageType>> logger, IAmazonSQS amazonSqs)
    {
        _amazonSqs = amazonSqs;
        _logger = logger;
    }

    protected abstract string QueueUrl { get; }
    protected virtual int DelayAfterNoMessage => 60000;
    protected virtual int MaxParallel => 4;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogDebug("Worker running at: {time}", DateTimeOffset.Now);
            var request = new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                MaxNumberOfMessages = MaxParallel,
            };
            var response = await _amazonSqs.ReceiveMessageAsync(request, cancellationToken);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Error receiving message from queue: {@response}", response);
                await Task.Delay(10000, cancellationToken);
                continue;
            }

            if (response.Messages.Count == 0)
            {
                _logger.LogInformation("No messages in queue");
                await Task.Delay(DelayAfterNoMessage, cancellationToken);
            }

            await ProcessMessages(response.Messages, cancellationToken);
        }
    }

    private async Task ProcessMessages(IEnumerable<Message> messages, CancellationToken cancellationToken)
    {
        await Task.WhenAll(messages.Select(message => LogAndProcessMessage(message, cancellationToken)).ToList());
    }

    private async Task LogAndProcessMessage(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing message: {@message}", message);
        try
        {
            var messageBody = JsonSerializer.Deserialize<TMessageType>(message.Body);
            if (messageBody == null)
            {
                _logger.LogError("Error deserializing message: {@message}", message);
                return;
            }

            var result = await ProcessMessage(messageBody, cancellationToken);
            switch (result)
            {
                case SqsMessageReturn.Success:
                    _logger.LogInformation("Deleting message: {@message}", message);
                    await _amazonSqs.DeleteMessageAsync(QueueUrl, message.ReceiptHandle, cancellationToken);
                    break;
                case SqsMessageReturn.Failure:
                    _logger.LogInformation("Error processing message: {@message}", message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (JsonException)
        {
            _logger.LogError("Error deserializing message: {@message}", message);
        }
        catch (Exception)
        {
            _logger.LogError("Error processing message: {@message}", message);
        }
    }
    protected abstract Task<SqsMessageReturn> ProcessMessage(TMessageType message, CancellationToken cancellationToken);
}
