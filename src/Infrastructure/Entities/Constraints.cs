namespace Infrastructure.Entities;

public static class Contraints
{
    public static string S3Bucket = "goj-submissions";
    public static string SubmissionsFolder = "submission_files";
    public static string SubmissionsQueueUrl = "https://sqs.us-east-1.amazonaws.com/459427504023/SubmissionsQueue";
    public static string SubmissionResultQueueUrl = "https://sqs.us-east-1.amazonaws.com/459427504023/SubmissionResult";
}