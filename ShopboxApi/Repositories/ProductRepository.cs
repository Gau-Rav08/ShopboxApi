using MongoDB.Bson;
using MongoDB.Driver;
using ShopboxApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopboxApi.Repositories
{
    public class ProductRepository
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Product> productCollection;
        public ProductRepository()
        {
            _client = new MongoClient("mongodb+srv://test:test@cluster0.uwxwn.mongodb.net/shopbox?retryWrites=true&w=majority");
            _database = _client.GetDatabase("shopbox");
            productCollection = _database.GetCollection<Product>("product");
        }
        public async Task<IEnumerable<Product>> GetNewProduct()
        {
            var filter = new BsonDocument();
            var sort = Builders<Product>.Sort.Descending("releasedDate");
            var result = await productCollection.Find(filter).Sort(sort).Limit(8).ToListAsync();
            return result;
        }
        public async Task<Product> GetProductById(string id)
        {
            var filter = Builders<Product>.Filter.Eq(e => e.Id, id);
            var result = await productCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        /*public async Task<Product> GetUserByEmail(string email)
        {
            var filter = Builders<Product>.Filter.Eq(e => e.Email, email);
            var result = await userCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }*/
        /*public async Task CreateUser(Product user)
        {
            await userCollection.InsertOneAsync(new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Phone = "",
                Address = ""
            });
        }*/
       /* public async Task UpdateUser(Product user)
        {
            var filter = Builders<Product>.Filter.Eq(e => e.Id, user.Id);
            var update = Builders<Product>.Update
                .Set(x => x.Name, user.Name)
                .Set(x => x.Email, user.Email)
                .Set(x => x.Password, user.Password)
                .Set(x => x.Phone, user.Phone)
                .Set(x => x.Address, user.Address);
            var result1 = await userCollection.UpdateOneAsync(filter, update);
        }*/
    }
}
