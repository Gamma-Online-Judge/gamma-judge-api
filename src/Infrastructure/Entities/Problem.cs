
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities;

public class Problem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? CustomId { get; set; }
    public string Title { get; set; } = "";
    public string Statment { get; set; } = "";
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string? ContestId { get; set; }
    public List<SampleInput> SampleInputs { get; set; } = new List<SampleInput>();
    public string Input { get; set; } = "";
    public string Output { get; set; } = "";
    public string Tutorial { get; set; } = "";
}

public class SampleInput
{
    public string Input { get; set; } = "";
    public string Output { get; set; } = "";
}