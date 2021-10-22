using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopboxApi.Dtos
{
    public class GetUserByIdDto
    {
        public string Id { get; set; }

    }
}
