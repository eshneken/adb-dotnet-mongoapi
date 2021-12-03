using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace adb_dotnet_mongoapi
{
    public class CarRepository
    {
        private readonly IMongoCollection<Car> carCollection;
        private readonly IMongoDatabase database;
        private readonly string repoName;

        public CarRepository(IMongoClient client, string dbName)
        {
            repoName = nameof(Car).ToLower();
            database = client.GetDatabase(dbName);
            carCollection = database.GetCollection<Car>(repoName);
        }

        public ObjectId Create(Car car)
        {
            carCollection.InsertOne(car);
            return car.Id;
        }

        public bool Delete(ObjectId objectId)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Id, objectId);
            var result = carCollection.DeleteOne(filter);
            return result.DeletedCount == 1;
        }

        public Car Get(ObjectId objectId)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Id, objectId);
            var car = carCollection.Find(filter).FirstOrDefault();
            return car;
        }

        public IEnumerable<Car> Get()
        {
            var cars = carCollection.Find(_ => true).ToList();
            return cars;
        }

        public  IEnumerable<Car> GetByMake(string make)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Make, make);
            var cars = carCollection.Find(filter).ToList();
            return cars;
        }

        public bool Update(ObjectId objectId, Car car)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Id, objectId);
            var update = Builders<Car>.Update
                .Set(c => c.Make, car.Make)
                .Set(c => c.Model, car.Model)
                .Set(c => c.Cylinders, car.Cylinders);
            var result = carCollection.UpdateOne(filter, update);
            return result.ModifiedCount == 1;
        }

        public void Drop()
        {
            database.DropCollection(repoName);
        }
    }
}