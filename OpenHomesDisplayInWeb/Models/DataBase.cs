using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHomesDisplayInWeb
{
    class SaveToDB
    {
        public void Insert(List<Region> regions)
        {
            //OpenHomeDBContext db = new OpenHomeDBContext();
            using (OpenHomeDBContext db = new OpenHomeDBContext())
            {
                var regionListFromDB = db.RegionModels.ToList();
                if (regionListFromDB.Count > 0)
                {
                    foreach (var item in regionListFromDB)
                    {
                        db.RegionModels.Remove(item);
                        db.SaveChanges();
                    }
                }

                foreach (Region region in regions)
                {
                    db.RegionModels.Add(region);
                    db.SaveChanges();
                }
            }
        }
    }

    class GetData
    {
        public void GetDataFromDB()
        {
            using (OpenHomeDBContext db = new OpenHomeDBContext())
            {
                var suburbListFromDB = db.SuburbModels.ToList();
                Console.WriteLine("------------------");
                Console.WriteLine("------------------");
                Console.WriteLine("FROM DB");

                foreach (var suburb in suburbListFromDB)
                {
                    Console.WriteLine(suburb.SuburbName);
                    Console.WriteLine("------------------");
                    var homesListFromDB = db.OpenHomeModels.Where(s => s.Suburb.SuburbName == suburb.SuburbName).ToList();
                    int homeCount = 0;
                    foreach (var home in homesListFromDB)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Home " + ++homeCount);
                        Console.WriteLine("Location = " + home.Location);
                        Console.WriteLine("Rooms = " + home.Rooms);
                        Console.WriteLine("Property Type = " + home.PropertyType);
                        Console.WriteLine("Price = " + home.Price);
                        Console.WriteLine("Parking = " + home.Parking);
                        Console.WriteLine("Open Home Time = " + home.OpenHomeTime);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
