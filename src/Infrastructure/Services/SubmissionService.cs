using Infrastructure.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using Amazon.S3;

namespace Infrastructure.Services;

public static class Contraints
{
    public static string SubmissionsBucket = "gama-judge-submissions";
    public static string FilesFolder = "submission_files";
}

public class SubmissionService
{
    private readonly IMongoCollection<Submission> _submissions;
    private readonly IAmazonS3 _s3Client;

    public SubmissionService(IJudgeDatabaseSettings settings, IAmazonS3 s3Client)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _submissions = database.GetCollection<Submission>(settings.SubmissionsCollectionName);
        _s3Client = s3Client;
    }

    public bool Exists(string? id) =>
        _submissions.Find<Submission>(submission => submission.Id == id).Any();

    public List<Submission> Get() =>
        _submissions.Find(submission => true).ToList();

    public Submission Get(string id) =>
        _submissions.Find<Submission>(submission => submission.Id == id).FirstOrDefault();

    public async Task<Submission> Create(Submission submission, Stream stream, CancellationToken cancellationToken)
    {
        submission.Id = ObjectId.GenerateNewId().ToString();
        await _s3Client.UploadObjectFromStreamAsync(Contraints.SubmissionsBucket, $"{Contraints.FilesFolder}/{submission.FileKey}", stream, null, cancellationToken);
        await _submissions.InsertOneAsync(submission);
        return submission;
    }

    public async Task<Stream> GetFile(Submission submission, CancellationToken cancellationToken)
    {
        var file = await _s3Client.GetObjectAsync(Contraints.SubmissionsBucket, $"{Contraints.FilesFolder}/{submission.FileKey}", cancellationToken);
        if (file is null) throw new FileNotFoundException();
        return file.ResponseStream;
    }

    public async Task<Stream> GetFile(string submissionId, CancellationToken cancellationToken)
    {
        var submission = Get(submissionId);
        return await GetFile(submission, cancellationToken);
    }
}
