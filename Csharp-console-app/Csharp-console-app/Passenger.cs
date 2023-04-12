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
        public static Age UNKNOWN_AGE = new Age(-1, -1);
    }

    public class Passenger
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public double Age { get; set; }
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
        // STILL needs to be fixed
        public static double AgeParse(string ageString)
        {
            string pattern = @"-?\d*\.?\d+";
            Match match = Regex.Match(ageString, pattern);

            if (match.Success)
            {
                double number = double.Parse(match.Value);

                if (number < 0)
                    return Global.UNKNOWN_VALUE;

                if (ageString.ToLower().StartsWith("age"))
                    return number;

                else if (ageString.ToLower().StartsWith("infant in months"))
                    return number / 12;
            }


            return Global.UNKNOWN_VALUE;  // for unknown age
        }
        public override string ToString()
        {
            if (Age != Global.UNKNOWN_VALUE)
                return $"{FirstName} {LastName} aged {Age:0.}";
            else
                return $"{FirstName} {LastName} (age unknown)";
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
                Ship? ship = uniqueShips.Find(s => s.ShipID == passenger.Ship.ShipID);

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

            // instead of typing
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

    }

    public class Ship
    {
        public string DestinationCountry { get; set; }
        public string DepartureSeaport { get; set; }
        public string ShipID { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public int Count { get; set; }

        public Ship(string destinationCountry, string departureSeaport, string shipID, DateOnly arrivalDate)
        {
            DestinationCountry = destinationCountry;
            DepartureSeaport = departureSeaport;
            ShipID = shipID;
            ArrivalDate = arrivalDate;
            // no Count assignment
        }

    }

    public class Age
    {
        // I don't think this class is needed, I'm keeping it just as an idea
        public int Years { get; set; }
        public int Months { get; set; }

        public Age(int years, int months) 
        { 
            Years = years;
            Months = months;
        }

        public static Age AgeParse2(string ageString)
        {
            string pattern = @"-?\d*\.?\d+";
            Match match = Regex.Match(ageString, pattern);

            if (!match.Success)
                return Global.UNKNOWN_AGE;

            bool intSuccess = int.TryParse(match.Value, out int num);

            if (!intSuccess)
                return Global.UNKNOWN_AGE;

            if (num < 0)
                return Global.UNKNOWN_AGE;


            if (ageString.ToLower().StartsWith("age"))
                return new Age(num, 0);
            else if (ageString.ToLower().StartsWith("infant in months"))
                return new Age(0, num);

            else
                return Global.UNKNOWN_AGE;
        }

        public override string ToString()
        {
            if (Months == 0)
                return $"{Years}";
            else if (Years == 0)
                return $"{Months} months";
            else
                return $"{Years} and {Months} months";
        }
    }

}
