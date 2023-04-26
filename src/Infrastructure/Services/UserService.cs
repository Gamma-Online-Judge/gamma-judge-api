using Infrastructure.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    private PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

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

    public User Get(string username) {
        var projection = Builders<User>.Projection.Exclude(d => d.Password);
        var user = _users.Find<User>(User => User.Username == username).Project<User>(projection).FirstOrDefault();
        return user;
    }
    
    public User Get(string username, string password) {
        User user = _users.Find<User>(User => User.Username == username).FirstOrDefault();
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(username, user.Password, password);
        if (passwordVerificationResult == PasswordVerificationResult.Success) 
            return user;
        else
            return null;
    }

    public User Create(User User)
    {
        User.Id = ObjectId.GenerateNewId().ToString();
        if (User.Username is null) throw new InvalidIdException(User.Username);
        if (Exists(User.Username)) throw new IdAlreadyExists(User.Username);
        User.Password = _passwordHasher.HashPassword(User.Username, User.Password);
        _users.InsertOne(User);

        return this.Get(User.Username);
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
