using Infrastructure.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IJudgeDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _users = database.GetCollection<User>(settings.UsersCollectionName);
    }

    public bool Exists(string? username) =>
        _users.Find<User>(User => User.Username == username).Any();

    public List<User> Get() =>
        _users.Find(User => true).ToList();

    public User Get(string username) =>
        _users.Find<User>(User => User.Username == username).FirstOrDefault();
    
        public User Get(string username, string password) =>
        _users.Find<User>(User => User.Username == username && User.Password == password).FirstOrDefault();

    public User Create(User User)
    {
        User.Id = ObjectId.GenerateNewId().ToString();
        if (User.Username is null) throw new InvalidIdException(User.Username);
        if (Exists(User.Username)) throw new IdAlreadyExists(User.Username);

        _users.InsertOne(User);
        return User;
    }

    public User CreateOrUpdate(User User)
    {
        if (User.Username is null) throw new InvalidIdException(User.Username);
        if (Exists(User.Username))
        {
            Update(User.Username, User);
            return Get(User.Username);
        }
        return Create(User);
    }

    public void Update(string id, User UserIn)
    {
        var User = Get(id);
        UserIn.Id = User.Id;
        _users.ReplaceOne(User => User.Username == id, UserIn);
    }

    public void Remove(User UserIn) =>
        _users.DeleteMany(User => User.Username == UserIn.Id);

    public void Remove(string id) =>
        _users.DeleteMany(User => User.Username == id);
}
