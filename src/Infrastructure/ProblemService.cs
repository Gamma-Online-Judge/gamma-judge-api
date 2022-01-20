using Infrastructure.Entities;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace contestsApi.Services
{
    public class ProblemService
    {
        private readonly IMongoCollection<Problem> _problems;

        public ProblemService(IJudgeDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _problems = database.GetCollection<Problem>(settings.ProblemsCollectionName);
        }

        public List<Problem> Get() =>
            _problems.Find(_ => true).ToList();

        public Problem Get(string id) =>
            _problems.Find<Problem>(problem => problem.CustomId == id).FirstOrDefault();

        public Problem Create(Problem problem)
        {
            if (problem.Id is null)
            {
                problem.Id = ObjectId.GenerateNewId().ToString();
            }
            _problems.InsertOne(problem);
            return problem;
        }

        public void Update(string id, Problem problemIn) =>
            _problems.ReplaceOne(problem => problem.CustomId == id, problemIn);

        public void Remove(Problem problemIn) =>
            _problems.DeleteOne(problem => problem.CustomId == problemIn.Id);

        public void Remove(string id) =>
            _problems.DeleteOne(problem => problem.CustomId == id);
    }
}