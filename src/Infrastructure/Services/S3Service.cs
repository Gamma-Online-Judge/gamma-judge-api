using Amazon.S3;

namespace Infrastructure.Services;

public class S3Service
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

    public async Task<string> SubmitFile(string fileName, Stream stream, CancellationToken cancellationToken)
    {
        var bucketFileName = $"{Guid.NewGuid().ToString()}--{fileName}";
        await _s3Client.UploadObjectFromStreamAsync(Contraints.SubmissionsBucket, $"{Contraints.FilesFolder}/{bucketFileName}", stream, null, cancellationToken);
        return bucketFileName;
    }

    public async Task<Stream> GetSubmissionFile(string fileKey, CancellationToken cancellationToken)
    {
        var file = await _s3Client.GetObjectAsync(Contraints.SubmissionsBucket, $"{Contraints.FilesFolder}/{fileKey}", cancellationToken);
        if (file is null) throw new FileNotFoundException();

        return file.ResponseStream;
    }
}