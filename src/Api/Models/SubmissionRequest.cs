using Infrastructure.Entities;

namespace Api.Models;

public class SubmissionRequest
{
    public string? ProblemId { get; set; }
    public string? Language { get; set; }
    public string? UserId { get; set; }

    public Submission ToSubmission(string fileName)
    {
        if(Language is null) throw new ArgumentNullException(nameof(Language));
        if(ProblemId is null) throw new ArgumentNullException(nameof(ProblemId));
        if(UserId is null) throw new ArgumentNullException(nameof(UserId));

        return new Submission
        {
            UserId = UserId,
            ProblemId = ProblemId,
            Language = Language.ToSubmissionLanguage(),
        };
    }
}