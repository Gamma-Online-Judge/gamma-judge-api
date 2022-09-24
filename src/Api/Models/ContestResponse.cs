using Infrastructure.Entities;

namespace Api.Models;

public class ContestResponse
{
    public string Id { get; set; }

    public string Name { get; set; } = "";
    public DateTime? Date { get; set; }
    public string CustomId { get; set; } = "";
    public List<ContestProblemResponse> Problems { get; set; } = new List<ContestProblemResponse>();

    public ContestResponse(Contest contest, List<ProblemShortResponse> problems)
    {
        var problemsByCustomId = problems.ToDictionary(problem => problem.CustomId ?? "");
        Id = contest.Id ?? "";
        Name = contest.Name;
        Date = contest.Date;
        CustomId = contest.CustomId;
        Problems = contest.Problems.Select(contestProblem => new ContestProblemResponse(contestProblem.Identifier, problemsByCustomId[contestProblem.CustomId])).ToList();
    }
}

public class ContestProblemResponse : ProblemShortResponse
{
    public string Identifier { get; set; } = "";

    public ContestProblemResponse(string identifier, ProblemShortResponse problem) : base(problem)
    {
        Identifier = identifier;
    }
}

public class OriginContestResponse
{
    public string CustomId { get; init; }
    public string Title { get; init; }
    public OriginContestResponse(Contest contest)
    {
        CustomId = contest.CustomId;
        Title = contest.Name;
    }
}