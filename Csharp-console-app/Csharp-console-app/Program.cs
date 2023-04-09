using System.Text.RegularExpressions;

namespace Csharp_console_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"../../../../../faminefiletoanalyse.csv";

            List<Passenger> passengerList = GetPassengerData(filePath);





            List<Ship> ships = passengerList.GetShips();

            foreach (Ship ship in ships)
                Console.WriteLine($"{ship.ShipID} arrived on {ship.ArrivalDate} WITH COUNT {ship.Count}");


            List<Passenger> filtered = passengerList.GetPassengersOnShip(ships[0]);

            Console.WriteLine($"{ships[0].ShipID} leaving from {ships[0].DepartureSeaport}\nArrived: {ships[0].ArrivalDate} with {filtered.Count} passengers");

            foreach(Passenger passenger in filtered)
                Console.WriteLine(passenger.ToString());

            Console.WriteLine(filtered.Count);





            List<(string, int)> occupations = passengerList.GetOccupationsAndAmounts();

            foreach(var item in occupations)
            {
                var (occupation, count) = item;
                Console.WriteLine($"uhh {occupation} and {count}");
            }




            
            List<(string, int)> ageGroups = new List<(string, int)>() 
            { 
                // the number represents the lower bound for your age group
                ("Infant", 0), 
                ("Child", 1), 
                ("Teen", 12), 
                ("Young Adult", 20),
                ("Adult", 30),
                ("Older Adult", 50)
            };

            List<(string, int, int)> ageGroupAmounts = passengerList.GetAgeGroupAmounts(ageGroups);

            foreach(var item in ageGroupAmounts)
            {
                var (name, age, amount) = item;
                if (age != Global.UNKNOWN_VALUE)
                    Console.WriteLine($"{name} >{age}: {amount}");
                else
                    Console.WriteLine($"{name}: {amount}");
            }


        }



        static List<Passenger> GetPassengerData(string path)
        {
            List<Passenger> passengers = new List<Passenger>();

            using (StreamReader sr = File.OpenText(path))
            {
                // read and discard the first line
                sr.ReadLine();

                string? s;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] fields = s.Split(",");

                    Passenger passenger = new()  // wack syntax
                    {
                        // the current implementation depends on the correct ordering of the columns
                        // it's possible to make it depend on the header row names
                        // but I'm going with the former, since changing the header row is easier than rearranging the columns

                        LastName = fields[0],
                        FirstName = fields[1],
                        Age = Passenger.AgeParse(fields[2]),
                        Sex = fields[3],
                        Occupation = fields[4],
                        NativeCountry = fields[5],
                        Ship = new Ship(
                            fields[6],
                            fields[7],
                            fields[8],
                            fields[9]
                            )
                    };

                    passengers.Add(passenger);

                    // debug stuff
                    // Console.WriteLine(passenger.ToString());
                }
            }

            return passengers;
        }
    }
}