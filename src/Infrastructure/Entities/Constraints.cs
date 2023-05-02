namespace Infrastructure.Entities;

public static class Contraints
{
    public static string S3Bucket = "gojsubmissions";
    public static string SubmissionsFolder = "submission_files";
    public static string SubmissionsQueueUrl = "https://sqs.us-east-1.amazonaws.com/829654486336/SubmissionResult";
    public static string SubmissionResultQueueUrl = "https://sqs.us-east-1.amazonaws.com/829654486336/SubmissionsQueue";
}