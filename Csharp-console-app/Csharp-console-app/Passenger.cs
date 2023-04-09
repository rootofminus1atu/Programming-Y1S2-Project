using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Csharp_console_app
{
    public class Global
    {
        public const int UNKNOWN_VALUE = -1;
    }

    public class Passenger
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public double? Age { get; set; }
        public string Sex { get; set; }
        public string Occupation { get; set; }
        public string NativeCountry { get; set; }
        public Ship Ship { get; set; }


        public Passenger() { }

        public Passenger(string lastName, string firstName, double age, string sex, string occupation, string nativeCountry, Ship ship)
        {
            LastName = lastName;
            FirstName = firstName;
            Age = age;
            Sex = sex;
            Occupation = occupation;
            NativeCountry = nativeCountry;
            Ship = ship;
        }

        // this below is ugly 
        // fix later
        public static double AgeParse(string ageString)
        {
            string pattern = @"\d+";


            if (ageString.StartsWith("age"))
            {
                Match match = Regex.Match(ageString, pattern);
                double number = double.Parse(match.Value);
                return number;
            }

            else if (ageString.StartsWith("Infant"))
            {
                Match match = Regex.Match(ageString, pattern);
                double number = double.Parse(match.Value);
                return number / 12;
            }


            return Global.UNKNOWN_VALUE;  // for unknown age
        }

        public override string ToString()
        {
            if (Age != Global.UNKNOWN_VALUE)
                return $"{LastName} {FirstName} aged {Age}";

            else
                return $"{LastName} {FirstName} (age unknown)";
        }
    }

    public static class PassengerExtension
    {
        // the implementation below gets only a list of unique ships, it tells us nothing about how many passengers there are on each
        public static List<Ship> GetShips(this List<Passenger> passengers)
        {
            List<Ship> ships = passengers
                .Select(x => x.Ship)
                .GroupBy(x => x.ShipID)
                .Select(x => x.First())  // this is wacky but it works
                .ToList();

            return ships;
        }

        // questionable implementation
        public static List<Ship> GetShips2(this List<Passenger> passengers)
        {
            List<Ship> uniqueShips = new List<Ship>() { };

            foreach (Passenger passenger in passengers)
            {
                Ship ship = uniqueShips.Find(s => s.ShipID == passenger.Ship.ShipID);

                if (ship == null)
                {
                    ship = new Ship(
                        passenger.Ship.DestinationCountry,
                        passenger.Ship.DepartureSeaport,
                        passenger.Ship.ShipID,
                        passenger.Ship.ArrivalDate
                        );

                    uniqueShips.Add(ship);
                }

                ship.Count++;
            }

            return uniqueShips;
        }

        public static List<Passenger> GetPassengersOnShip(this List<Passenger> passengers, Ship ship)
        {
            List<Passenger> theChosenOnes = passengers
                .Where(p => p.Ship.ShipID == ship.ShipID)
                .ToList();

            return theChosenOnes;

            // this could be used to get the count too! pretty easily
            //
            // also instead of typing
            // p.Ship.ShipID == ship.ShipID
            // I could implement ship equality
        }

        public static List<(string, int)> GetOccupationsAndAmounts(this List<Passenger> passengers)
        {
            List<(string, int)> occupations = passengers
                .Select(p => p.Occupation)
                .GroupBy(x => x)
                .Select(x => (x.Key, x.Count()))
                .ToList();

            return occupations;
        }


        public static List<(string, int, int)> GetAgeGroupAmounts(this List<Passenger> passengers, List<(string, int)> ageGroups)
        {
            List<(string, int, int)> ageGroupsWithAmounts = new List<(string, int, int)>() { };

            for(int i = 0; i < ageGroups.Count; i++)
            {
                var (name, ageLower) = ageGroups[i];
                var (_, ageUpper) = i >= ageGroups.Count - 1 ? (null, int.MaxValue) : ageGroups[i + 1];

                int amount = passengers
                    .Select(p => p.Age)
                    .Where(a => a >= ageLower && a < ageUpper)
                    .Count();

                ageGroupsWithAmounts.Add((name, ageLower, amount));
            }

            int unknownAmount = passengers
                .Select(p => p.Age)
                .Where(a => a == Global.UNKNOWN_VALUE)
                .Count();

            ageGroupsWithAmounts.Add(("Unknown", Global.UNKNOWN_VALUE, unknownAmount));

            return ageGroupsWithAmounts;
        }


        /*
        public static List<int> GetAgeRangeNumbers2(this List<Passenger> passengers, List<int> ageRanges)
        {
            List<int> counts = new List<int>() { };
            
            for(int i = 0; i < passengers.Count; i++)
            {
                for(int j = 0; j < ageRanges.Count; j++)
                {
                    int lower = ageRanges[i - 1]
                }
            }
            
            return counts;
        }
        */
    }

    public class Ship
    {
        public string DestinationCountry { get; set; }
        public string DepartureSeaport { get; set; }
        public string ShipID { get; set; }
        public string ArrivalDate { get; set; }
        public int Count { get; set; }

        public Ship(string destinationCountry, string departureSeaport, string shipID, string arrivalDate)
        {
            DestinationCountry = destinationCountry;
            DepartureSeaport = departureSeaport;
            ShipID = shipID;
            ArrivalDate = arrivalDate;
            // no Count assignment
        }

    }

    public class Report
    {
        public List<Passenger> Passengers { get; set; }

        public Report(List<Passenger> passengers)
        {
            Passengers = passengers;
        }
    }

}
