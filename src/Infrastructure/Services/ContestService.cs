using Infrastructure.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class ContestService
{
    private readonly IMongoCollection<Contest> _contests;

    public ContestService(IJudgeDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _contests = database.GetCollection<Contest>(settings.ContestsCollectionName);
    }

    public bool Exists(string? id) =>
        _contests.Find<Contest>(contest => contest.CustomId == id).Any();

    public List<Contest> Get() =>
        _contests.Find(contest => true).ToList();

    public Contest Get(string id) =>
        _contests.Find<Contest>(contest => contest.CustomId == id).FirstOrDefault();

    public Contest Create(Contest contest)
    {
        contest.Id = ObjectId.GenerateNewId().ToString();
        if (contest.CustomId is null) throw new InvalidIdException(contest.CustomId);
        if (Exists(contest.CustomId)) throw new IdAlreadyExists(contest.CustomId);

        _contests.InsertOne(contest);
        return contest;
    }

    public Contest CreateOrUpdate(Contest contest)
    {
        if (contest.CustomId is null) throw new InvalidIdException(contest.CustomId);
        if (Exists(contest.CustomId))
        {
            Update(contest.CustomId, contest);
            return Get(contest.CustomId);
        }
        return Create(contest);
    }

    public void Update(string id, Contest contestIn)
    {
        var contest = Get(id);
        contestIn.Id = contest.Id;
        _contests.ReplaceOne(contest => contest.CustomId == id, contestIn);
    }

    public void Remove(Contest contestIn) =>
        _contests.DeleteMany(contest => contest.CustomId == contestIn.Id);

    public void Remove(string id) =>
        _contests.DeleteMany(contest => contest.CustomId == id);

    public List<Contest> QueryByName(string name = "", int limit = 10, int skip = 0){
        return _contests.Find(contest => contest.Name.ToLower().Contains(name.ToLower()))
            .Limit(limit)
            .Skip(skip)
            .ToList();
    }
}
