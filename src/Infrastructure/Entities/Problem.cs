
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities;

public class Problem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? CustomId { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string? ContestId { get; set; }
    public List<SampleInput> SampleInputs { get; set; } = new List<SampleInput>();
    public SampleLanguageInput Pt_BR { get; set; } = new SampleLanguageInput();
    public List<SecretTestInput> SecretTests { get; set; } = new List<SecretTestInput>();
}

public class SampleInput
{
    public string Input { get; set; } = "";
    public string Output { get; set; } = "";
}

public class SampleLanguageInput
{
    public string Title { get; set; } = "";
    public string Statement { get; set; } = "";
    public string Input { get; set; } = "";
    public string Output { get; set; } = "";
    public string Tutorial { get; set; } = "";
    public string Notes { get; set; } = "";
}

public class SecretTestInput
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id {get; set;} = "";
    public string Filename {get; set;} = "";
}