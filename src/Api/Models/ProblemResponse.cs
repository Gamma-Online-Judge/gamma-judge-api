using Infrastructure.Entities;

namespace Api.Models;

public class ProblemResponse
{
    public string Id { get; init; }
    public string CustomId { get; init; }
    public string Title { get; init; }
    public string Statment { get; init; }
    public int TimeLimit { get; init; }
    public int MemoryLimit { get; init; }
    public List<string> Tags { get; init; } = new List<string>();
    public List<SampleInput> SampleInputs { get; init; } = new List<SampleInput>();
    public string Input { get; init; }
    public string Output { get; init; }
    public string Tutorial { get; init; }
    public string Notes { get; init; }
    public OriginContestResponse? OriginContest { get; init; }
    

    public ProblemResponse(Problem problem, Contest? originContest)
    {
        Id = problem.Id ?? "";
        CustomId = problem.CustomId ?? ""; ;
        Title = problem.Title;
        Statment = problem.Statment;
        TimeLimit = problem.TimeLimit;
        MemoryLimit = problem.MemoryLimit;
        Tags = problem.Tags;
        SampleInputs = problem.SampleInputs;
        Input = problem.Input;
        Output = problem.Output;
        Tutorial = problem.Tutorial;
        OriginContest = originContest == null ? null : new OriginContestResponse(originContest);
        Notes = problem.Notes;
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
        Title = problem.Title;
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
