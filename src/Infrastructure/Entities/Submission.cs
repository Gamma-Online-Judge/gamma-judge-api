
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

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
    public string SubmissionDetails { get; set; } = "";
    public string FileKey => $"{Id}.{Language.GetExtension()}";
}

[JsonConverter(typeof(StringEnumConverter))]
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

[JsonConverter(typeof(StringEnumConverter))]
public enum SubmissionLanguage
{
    [EnumMember(Value = "c")]
    C,
    [EnumMember(Value = "cpp")]
    Cpp,
    [EnumMember(Value = "go")]
    Go,
    [EnumMember(Value = "java")]
    Java,
    [EnumMember(Value = "js")]
    JavaScript,
    [EnumMember(Value = "ml")]
    ML,
    [EnumMember(Value = "pas")]
    Pascal,
    [EnumMember(Value = "py2")]
    Python2,
    [EnumMember(Value = "py3")]
    Python3,
    [EnumMember(Value = "riscv")]
    RiscV,
    [EnumMember(Value = "rs")]
    Racket,
    [EnumMember(Value = "sh")]
    Bash,
    [EnumMember(Value = "spim")]
    Spim
}

public static class SubmissionExtensions
{
    public static string GetExtension(this SubmissionLanguage language)
    {
        return language switch
        {
            SubmissionLanguage.C => "c",
            SubmissionLanguage.Cpp => "cpp",
            SubmissionLanguage.Go => "go",
            SubmissionLanguage.Java => "java",
            SubmissionLanguage.JavaScript => "js",
            SubmissionLanguage.ML => "ml",
            SubmissionLanguage.Pascal => "pas",
            SubmissionLanguage.Python2 => "py2",
            SubmissionLanguage.Python3 => "py3",
            SubmissionLanguage.RiscV => "riscv",
            SubmissionLanguage.Racket => "rs",
            SubmissionLanguage.Bash => "sh",
            SubmissionLanguage.Spim => "spim",
            _ => throw new ArgumentException("Invalid language")
        };
    }
    public static SubmissionLanguage ToSubmissionLanguage(this string language)
    {
        return language switch
        {
            "c" => SubmissionLanguage.C,
            "cpp" => SubmissionLanguage.Cpp,
            "go" => SubmissionLanguage.Go,
            "java" => SubmissionLanguage.Java,
            "js" => SubmissionLanguage.JavaScript,
            "ml" => SubmissionLanguage.ML,
            "pas" => SubmissionLanguage.Pascal,
            "py2" => SubmissionLanguage.Python2,
            "py3" => SubmissionLanguage.Python3,
            "riscv" => SubmissionLanguage.RiscV,
            "rs" => SubmissionLanguage.Racket,
            "sh" => SubmissionLanguage.Bash,
            "spim" => SubmissionLanguage.Spim,
            _ => throw new ArgumentException("Invalid language")
        };
    }

    public static SubmissionStatus ToSubmissionStatus(this string details)
    {
        if (details.Contains("Accepted"))
        {
            return SubmissionStatus.Accepted;
        }
        else if (details.Contains("Time Limit Exceeded"))
        {
            return SubmissionStatus.TimeLimitExceeded;
        }
        else if (details.Contains("Runtime Error"))
        {
            return SubmissionStatus.RuntimeError;
        }
        else if (details.Contains("Wrong Answer"))
        {
            return SubmissionStatus.WrongAnswer;
        }
        else if (details.Contains("Compilation Error"))
        {
            return SubmissionStatus.CompilationError;
        }
        else if (details.Contains("Memory Limit Exceeded"))
        {
            return SubmissionStatus.MemoryLimitExceeded;
        }
        else if (details.Contains("InQueue"))
        {
            return SubmissionStatus.InQueue;
        }
        else if (details.Contains("Pending"))
        {
            return SubmissionStatus.Pending;
        }
        else if (details.Contains("Running"))
        {
            return SubmissionStatus.Running;
        }
        else
        {
            throw new ArgumentException($"Invalid details: {details}");
        };
    }
}