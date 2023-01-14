using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Models
{
    public class User
    {
        public string Date_time { get; set; } = null!;
        public string IPAddress { get; set; } = null!;

    }
}
