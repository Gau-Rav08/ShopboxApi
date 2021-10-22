using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopboxApi.Models
{

    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("brand")]
        public string Brand { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("price")]
        public long Price { get; set; }

        [BsonElement("ram")]
        public int Ram { get; set; }

        [BsonElement("rom")]
        public int Rom { get; set; }

        [BsonElement("battery")]
        public long Battery { get; set; }

        [BsonElement("screen")]
        public string Screen { get; set; }

        [BsonElement("processor")]
        public string Processor { get; set; }

    }
}
