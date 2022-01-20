using Infrastructure.Entities;

namespace Api.Models;

public class SubmissionRequest
{
    public string? ProblemId { get; set; }
    public string? Language { get; set; }
    public string? UserId { get; set; }
    public ICollection<IFormFile>? Files { get; set; }
    public IFormFile? File => Files?.FirstOrDefault();

    public Submission ToSubmission()
    {
        if(Language is null) throw new ArgumentNullException(nameof(Language));
        if(ProblemId is null) throw new ArgumentNullException(nameof(ProblemId));
        if(UserId is null) throw new ArgumentNullException(nameof(UserId));
        if(File is null) throw new ArgumentNullException(nameof(File));

        return new Submission
        {
            UserId = UserId,
            ProblemId = ProblemId,
            Language = Language.ToSubmissionLanguage(),
            FileName = File.FileName,
        };
    }
}