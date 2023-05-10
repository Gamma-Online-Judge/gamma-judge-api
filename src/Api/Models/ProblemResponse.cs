using Infrastructure.Entities;

namespace Api.Models;

public class ProblemResponse
{
    public string Id { get; init; }
    public string CustomId { get; init; }
    public string Title { get; init; }
    public int TimeLimit { get; init; }
    public int MemoryLimit { get; init; }
    public List<string> Tags { get; init; } = new List<string>();
    public List<SampleInput> SampleInputs { get; init; } = new List<SampleInput>();
    public OriginContestResponse? OriginContest { get; init; }
    public SampleLanguageInput Pt_BR { get; init; } = new SampleLanguageInput();
    

    public ProblemResponse(Problem problem, Contest? originContest)
    {
        Id = problem.Id ?? "";
        CustomId = problem.CustomId ?? ""; ;
        Pt_BR.Title = problem.Pt_BR.Title;
        Pt_BR.Statement = problem.Pt_BR.Statement;
        TimeLimit = problem.TimeLimit;
        MemoryLimit = problem.MemoryLimit;
        Tags = problem.Tags;
        SampleInputs = problem.SampleInputs;
        Pt_BR.Input = problem.Pt_BR.Input;
        Pt_BR.Output = problem.Pt_BR.Output;
        Pt_BR.Tutorial = problem.Pt_BR.Tutorial;
        OriginContest = originContest == null ? null : new OriginContestResponse(originContest);
        Pt_BR.Notes = problem.Pt_BR.Notes;
    }
}

public class ProblemShortResponse
{
    public string Id { get; init; }
    public string CustomId { get; init; }
    public string Title { get; init; }
    public List<string> Tags { get; init; } = new List<string>();
    public OriginContestResponse? OriginContest { get; init; }

    public ProblemShortResponse(Problem problem, Contest? originContest)
    {
        Id = problem.Id ?? "";
        CustomId = problem.CustomId ?? ""; ;
        Title = problem.Pt_BR.Title;
        Tags = problem.Tags;
        OriginContest = originContest == null ? null : new OriginContestResponse(originContest);
    }

    public ProblemShortResponse(ProblemShortResponse problem)
    {
        Id = problem.Id;
        CustomId = problem.CustomId;
        Title = problem.Title;
        Tags = problem.Tags;
        OriginContest = problem.OriginContest;
    }
}
