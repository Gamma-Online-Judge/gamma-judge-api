
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities;

public class Submission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? ProblemId { get; set; }
    public string? UserId { get; set; }
    public SubmissionLanguage Language { get; set; }
    public string FileName { get; set; } = "";
    public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;

    public string FileKey => $"{Id}.{Language.GetExtension()}";
}

public enum SubmissionStatus
{
    Accepted,
    WrongAnswer,
    TimeLimitExceeded,
    MemoryLimitExceeded,
    RuntimeError,
    CompilationError,
    InQueue,
    Pending,
    Running,
}
public enum SubmissionLanguage
{
    C,
    Cpp,
    Java,
    Python2,
    Python3
}

public static class SubmissionExtensions
{
    public static string GetExtension(this SubmissionLanguage language)
    {
        return language switch
        {
            SubmissionLanguage.C => "c",
            SubmissionLanguage.Cpp => "cpp",
            SubmissionLanguage.Java => "java",
            SubmissionLanguage.Python2 => "py2",
            SubmissionLanguage.Python3 => "py3",
            _ => throw new ArgumentException("Invalid language")
        };
    }
    public static SubmissionLanguage ToSubmissionLanguage(this string language)
    {
        return language switch
        {
            "c" => SubmissionLanguage.C,
            "cpp" => SubmissionLanguage.Cpp,
            "java" => SubmissionLanguage.Java,
            "py2" => SubmissionLanguage.Python2,
            "py3" => SubmissionLanguage.Python3,
            _ => throw new ArgumentException("Invalid language")
        };
    }
}