using Infrastructure.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using Amazon.S3;

namespace Infrastructure.Services;

public class ProblemService
{
    private readonly IMongoCollection<Problem> _problems;
    private readonly IAmazonS3 _s3Client;

    public ProblemService(
        IJudgeDatabaseSettings settings,
        IAmazonS3 s3Client
    )
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _problems = database.GetCollection<Problem>(settings.ProblemsCollectionName);
        _s3Client = s3Client;
    }

    public bool Exists(string? id) =>
        _problems.Find<Problem>(contest => contest.CustomId == id).Any();

    public List<Problem> Get() =>
        _problems.Find(_ => true).ToList();

    public Problem Get(string id) =>
        _problems.Find<Problem>(problem => problem.CustomId == id).FirstOrDefault();

    public Problem Create(Problem problem)
    {
        problem.Id = ObjectId.GenerateNewId().ToString();
        if (problem.CustomId is null) throw new InvalidIdException(problem.CustomId);
        if (Exists(problem.CustomId)) throw new IdAlreadyExists(problem.CustomId);

        _problems.InsertOne(problem);
        return problem;
    }

    public Problem CreateOrUpdate(Problem problem){
        if (problem.CustomId is null) throw new InvalidIdException(problem.CustomId);
        if (Exists(problem.CustomId)){
            Update(problem.CustomId, problem);
            return Get(problem.CustomId);
        }
        return Create(problem);
    }

    public void Update(string id, Problem problemIn){
        var problem = Get(id);
        problemIn.Id = problem.Id;
        _problems.ReplaceOne(problem => problem.CustomId == id, problemIn);
    }

    public void Remove(Problem problemIn) =>
        _problems.DeleteOne(problem => problem.CustomId == problemIn.Id);

    public void Remove(string id) =>
        _problems.DeleteOne(problem => problem.CustomId == id);

    public List<Problem> QueryByTitle(string title = "", int limit = 10, int skip = 0){
        return _problems.Find(problem => problem.Pt_BR.Title.ToLower().Contains(title.ToLower()))
            .Limit(limit)
            .Skip(skip)
            .ToList();
    }

    public async Task<List<SecretTestInput>> AddTestCases(List<Stream> Files, string ProblemId, CancellationToken cancellationToken)
    { 
        var problem = this.Get(ProblemId);

        if (problem == null)
            throw new KeyNotFoundException($"Problem with id {ProblemId} not found");
        
        foreach (var File in Files) 
        {
            var secretTest = new SecretTestInput();
            secretTest.Id = ObjectId.GenerateNewId().ToString();
            secretTest.Filename = $"{Contraints.ProblemsFolder}/{ProblemId}/{secretTest.Id}.txt";
            await _s3Client.UploadObjectFromStreamAsync(Contraints.S3Bucket, secretTest.Filename, File, null, cancellationToken);

            problem.SecretTests.Add(secretTest);
        }

        this.Update(ProblemId, problem);

        return problem.SecretTests;
    }
}
