using Infrastructure.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class ProblemService
{
    private readonly IMongoCollection<Problem> _problems;

    public ProblemService(IJudgeDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _problems = database.GetCollection<Problem>(settings.ProblemsCollectionName);
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
        return _problems.Find(problem => problem.Title.ToLower().Contains(title.ToLower()))
            .Limit(limit)
            .Skip(skip)
            .ToList();
    }
}
