using System.Text.Json;
using Amazon.S3;

namespace Infrastructure.S3Service;

public interface IS3Service
{
    Task<IEnumerable<string>> ListObjects(string prefix, CancellationToken cancellationToken);
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
        return await _s3Client.GetAllObjectKeysAsync("gama-judge-submissions", prefix, null);
    }
}