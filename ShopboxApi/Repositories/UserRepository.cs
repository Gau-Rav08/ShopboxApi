using MongoDB.Driver;
using ShopboxApi.Models;
using System.Collections.Generic;
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

        public async Task<User> GetUserWhenLogin(string email, string pass)
        {
            var emailFilter = Builders<User>.Filter.Eq(e => e.Email, email);
            var passFilter = Builders<User>.Filter.Eq(e => e.Password, pass);
            var combineFilters = Builders<User>.Filter.And(emailFilter, passFilter);
            var result = await userCollection.Find(combineFilters).FirstOrDefaultAsync();
            return result;
        }
        public async Task<bool> CreateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(e => e.Email, user.Email);
            var result = await userCollection.Find(filter).FirstOrDefaultAsync();
            if (result != null)
            {
                return false;
            } 
            else
            {
                List<CartItem> cart = new();
                List<Wish> wish = new();
                await userCollection.InsertOneAsync(new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Phone = "",
                    Address = "",
                    Cart = cart ,
                    Wishlist = wish
                });
                return true;
            }
            
        }
        public async Task<User> UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(e => e.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Name, user.Name)
                .Set(x => x.Email, user.Email)
                .Set(x => x.Password, user.Password)
                .Set(x => x.Phone, user.Phone)
                .Set(x => x.Address, user.Address);
            await userCollection.UpdateOneAsync(filter, update);
            var result = await GetUserById(user.Id);
            return result;
        }

        public async Task FindAndUpdateCart(User user)
        {
            var filter = Builders<User>.Filter.Eq(e => e.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Cart, user.Cart);
            await userCollection.UpdateOneAsync(filter, update);
        }
        
    }
}
