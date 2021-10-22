using MongoDB.Driver;
using ShopboxApi.Models;
using System.Threading.Tasks;

namespace ShopboxApi.Repositories
{
    public class UserRepository
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> userCollection;
        public UserRepository()
        {
            _client = new MongoClient("mongodb+srv://test:test@cluster0.uwxwn.mongodb.net/shopbox?retryWrites=true&w=majority");
            _database = _client.GetDatabase("shopbox");
            userCollection = _database.GetCollection<User>("users");
        }
      /*  public async Task<IEnumerable<User>> GetAllEmployees()
        {
            var filter = new BsonDocument();
            var result = await userCollection.Find(filter).ToListAsync();
            return result;
        }*/
        public async Task<User> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq(e => e.Id, id);
            var result = await userCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(e => e.Email, email);
            var result = await userCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }
        public async Task CreateUser(User user)
        {
            await userCollection.InsertOneAsync(new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Phone = "",
                Address = ""
            });
        }
        public async Task UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(e => e.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Name, user.Name)
                .Set(x => x.Email, user.Email)
                .Set(x => x.Password, user.Password)
                .Set(x => x.Phone, user.Phone)
                .Set(x => x.Address, user.Address);
            var result1 = await userCollection.UpdateOneAsync(filter, update);
        }
       /* public async Task DeleteEmployee(string id)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, id);
            await userCollection.DeleteOneAsync(filter);
        }*/
    }
}
