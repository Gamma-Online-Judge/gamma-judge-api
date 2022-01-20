namespace Infrastructure.Entities;

public static class Contraints
{
    public static string S3Bucket = "gama-judge-submissions";
    public static string SubmissionsFolder = "submission_files";
    public static string SubmissionsQueueUrl = "https://sqs.sa-east-1.amazonaws.com/818598312538/SubmissionsQueue";
    public static string SubmissionResultQueueUrl = "https://sqs.sa-east-1.amazonaws.com/818598312538/SubmissionResult";
}