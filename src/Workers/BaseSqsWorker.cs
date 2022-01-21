using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Workers.Models;
using Newtonsoft.Json;

namespace Workers;

public abstract class BaseSqsWorker<TMessageType> : BackgroundService
{
    private class MessageShape
    {
        public string MessageId { get; set; } = "";
        public string Message { get; set; } = "";
    }

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
            var response = await _amazonSqs.ReceiveMessageAsync(QueueUrl, cancellationToken);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Error receiving message from queue: {@response}", response);
                await Task.Delay(10000, cancellationToken);
                continue;
            }

            if (response.Messages.Count == 0)
            {
                await Task.Delay(DelayAfterNoMessage, cancellationToken);
            }

            await ProcessMessages(response.Messages, cancellationToken);
        }
    }

    private async Task ProcessMessages(IEnumerable<Message> messages, CancellationToken cancellationToken)
    {
        await Task.WhenAll(messages.Select(message => LogAndProcessMessage(DesserializeMessage(message), message.ReceiptHandle, cancellationToken)).ToList());
    }

    private TMessageType DesserializeMessage(Message sqsMessage)
    {
        var messageShape = JsonConvert.DeserializeObject<MessageShape>(sqsMessage.Body);
        var result = JsonConvert.DeserializeObject<TMessageType>(messageShape?.Message ?? "");
        if (result is null)
            throw new JsonException("Message deserialization failed");
        return result;
    }

    private async Task LogAndProcessMessage(TMessageType message, string receiptHandle, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Processing message: {message}");
        try
        {
            var result = await ProcessMessage(message, cancellationToken);
            switch (result)
            {
                case SqsMessageReturn.Success:
                    _logger.LogInformation("Deleting message: {@message}", message);
                    await _amazonSqs.DeleteMessageAsync(QueueUrl, receiptHandle, cancellationToken);
                    break;
                case SqsMessageReturn.Failure:
                    _logger.LogInformation("Error processing message: {@message}", message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (JsonException e)
        {
            _logger.LogError($"Error deserializing message: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error processing message: {e.Message}");
        }
    }
    protected abstract Task<SqsMessageReturn> ProcessMessage(TMessageType message, CancellationToken cancellationToken);
}
