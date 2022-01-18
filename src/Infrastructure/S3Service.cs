using System.Text.Json;
using Amazon.S3;

namespace Infrastructure.S3Service;

public static class Contraints{
    public static string SubmissionsBucket = "gama-judge-submissions";
}

public interface IS3Service
{
    Task<IEnumerable<string>> ListObjects(string prefix, CancellationToken cancellationToken);
    Task<string> SubmitFile(string fileName, Stream stream, CancellationToken cancellationToken);
}

public class S3Service : IS3Service
{

    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<IEnumerable<string>> ListObjects(string prefix, CancellationToken cancellationToken)
    {
        return await _s3Client.GetAllObjectKeysAsync(Contraints.SubmissionsBucket, prefix, null);
    }

    public async Task<string> SubmitFile(string fileName, Stream stream, CancellationToken cancellationToken){
        var bucketFileName = $"{new Guid().ToString()}{fileName}";
        Console.WriteLine(bucketFileName);
        await _s3Client.UploadObjectFromStreamAsync(Contraints.SubmissionsBucket, $"submission_files/{bucketFileName}", stream, null, cancellationToken);
        return bucketFileName;
    }
}