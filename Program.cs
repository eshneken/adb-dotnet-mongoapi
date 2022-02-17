using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace adb_dotnet_mongoapi
{
    class Program
    {
        public static string ConnectString;
        public static MongoClient Client;
        public static string DBName;

        static void Main(string[] args)
        {
            // build the mongo config
            Program.createConfig();
            Client = new MongoClient(ConnectString);

            // clean the cars collection to make the demo idempotent
            Client.GetDatabase(DBName).DropCollection(nameof(Car).ToLower());

            // create cars repository and drop the existing collection
            CarRepository carRepo = new CarRepository(Client, DBName);

            // create the repo and add some cars to it
            Car mazda3 = new Car {Make="Mazda", Model="3", Cylinders=4};
            Car hondaCrv = new Car {Make="Honda", Model="CRV", Cylinders=4};
            Car hondaAccord = new Car {Make="Honda", Model="Accord", Cylinders=6};
            Car fordF150 = new Car {Make="Ford", Model="F150", Cylinders=6};
            Car nissanAltima = new Car {Make="Nissan", Model="Altima", Cylinders=4};

            var m3id = carRepo.Create(mazda3);
            var hcrvid = carRepo.Create(hondaCrv);
            var haccid = carRepo.Create(hondaAccord);
            var f150id = carRepo.Create(fordF150);
            var naltid = carRepo.Create(nissanAltima);

            Console.WriteLine("Inserted 5 vehicles into DB...");

            // get cars from DB and print
            printAllCars(carRepo);

            // update cylinders in altima from 4 -> 6 and show all cars
            Console.WriteLine("Updating Altima cylinders from 4 -> 6...");
            nissanAltima.Cylinders=6;
            carRepo.Update(naltid, nissanAltima);
            printAllCars(carRepo);

            // show all Hondas (filter by make)
            Console.WriteLine("Retrieving all Hondas...");
            foreach (var car in carRepo.GetByMake("Honda")) {
                Console.Write(car.Make + "-" + car.Model + "("+ car.Cylinders + ") ");
            }
            Console.WriteLine();

            // delete Mazda 3
            Console.WriteLine("Removing Mazda3 from DB...");
            carRepo.Delete(ObjectId.Parse(m3id.ToString()));
            printAllCars(carRepo);
        }

        public static void printAllCars(CarRepository carRepo) {
            Console.Write("Read all from DB -> Make-Model(Cylinders): ");
            foreach (var car in carRepo.Get()) {
                Console.Write(car.Make + "-" + car.Model + "("+ car.Cylinders + ") ");
            }
            Console.WriteLine();
        }

        public static void createConfig() {
            // connection settings
            var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables().Build();
            ConnectString = config.GetValue<string>("ConnectString");
            DBName = config.GetValue<string>("DBName");
        }
    }
}
