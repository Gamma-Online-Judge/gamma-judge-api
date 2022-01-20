
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities;

public class Contest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = "";
    public DateTime? Date { get; set; }
    public string CustomId { get; set; } = "";
    public List<ContestProblem> Problems { get; set; } = new List<ContestProblem>();
}

public class ContestProblem
{
    public string Identifier { get; set; } = "";
    public string CustomId { get; set; } = "";
}