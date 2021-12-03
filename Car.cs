using MongoDB.Bson;

namespace adb_dotnet_mongoapi
{
    public class Car
    {
        public ObjectId Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Cylinders { get; set; }
    }
}