using Assignment5.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Assignment5.Services;

public class UserService
{
    private readonly IMongoCollection<User> _UsersCollection;

    public UserService(
        IOptions<UserSettings> UserSettings)
    {
        var mongoClient = new MongoClient(
            UserSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            UserSettings.Value.DatabaseName);

        _UsersCollection = mongoDatabase.GetCollection<User>(
            UserSettings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetAsync() =>
        await _UsersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _UsersCollection.Find(x => x._id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _UsersCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser)=>
        await _UsersCollection.ReplaceOneAsync(x => x._id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _UsersCollection.DeleteOneAsync(x => x._id== id);
}