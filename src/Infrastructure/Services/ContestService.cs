using Infrastructure.Entities;
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

    public List<Contest> Get() =>
        _contests.Find(contest => true).ToList();

    public Contest Get(string id) =>
        _contests.Find<Contest>(contest => contest.CustomId == id).FirstOrDefault();

    public Contest Create(Contest contest)
    {
        if (contest.Id is null)
        {
            contest.Id = ObjectId.GenerateNewId().ToString();
        }
        _contests.InsertOne(contest);
        return contest;
    }

    public void Update(string id, Contest contestIn) =>
        _contests.ReplaceOne(contest => contest.CustomId == id, contestIn);

    public void Remove(Contest contestIn) =>
        _contests.DeleteOne(contest => contest.CustomId == contestIn.Id);

    public void Remove(string id) =>
        _contests.DeleteOne(contest => contest.CustomId == id);
}
