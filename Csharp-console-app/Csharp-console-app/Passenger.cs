/*
 *                         Passenger.cs
 * 
 *  Here you can find the classes responsible for storing
 *  and manipulating the data, mainly:
 *  - Passenger
 *  - PassengerExtension
 *  - Ship
 *  
 *  After the data is processed here it can be used in Program.cs
 *  
 */

using System.Text.RegularExpressions;

namespace Csharp_console_app
{
    /// <summary>
    /// Global constants and variables
    /// </summary>
    public class Global
    {
        public const int UNKNOWN_VALUE = -1;
        public static Age UNKNOWN_AGE = new Age(-1, -1);  // unused
    }


    /// <summary>
    /// Represents a passenger and their information
    /// </summary>
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

        /// <summary>
        /// Parses 
        /// </summary>
        /// <param name="passengers">A list of passengers</param>
        /// <returns>A list of all the unique ships from the Passenger list</returns>
        public static double AgeParse(string ageString)
        {
            string pattern = @"-?\d*\.?\d+";  // detects any number in a string
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
                return $"{FirstName} {LastName} aged {Age:0.###}";
            else
                return $"{FirstName} {LastName} (age unknown)";
        }
    }


    /// <summary>
    /// Provides methods that can be used on a list, array, or any other structure consisting of Passenger objects
    /// </summary>
    public static class PassengerExtension
    {
        /// <summary>
        /// Returns a list of unique ships that the passengers have traveled on
        /// </summary>
        /// <param name="passengers">A list of passengers</param>
        /// <returns>A list of all the unique ships from the Passenger list</returns>
        public static List<Ship> GetShips(this List<Passenger> passengers)
        {
            List<Ship> uniqueShips = passengers
                .Select(x => x.Ship)
                .GroupBy(x => x.ShipID)
                .Select(x => x.First())  // this is wacky but it works
                .ToList();

            return uniqueShips;
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


        /// <summary>
        /// Returns a list of passengers who have traveled on a specified ship
        /// </summary>
        /// <param name="passengers">A list of passengers</param>
        /// <param name="ship">The ship of interest</param>
        /// <returns>A list of passengers who have traveled on the specified ship</returns>
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


        /// <summary>
        /// Returns a list of tuples representing the number of passengers for each unique occupation
        /// </summary>
        /// <param name="passengers">A list of passengers</param>
        /// <returns>A list of tuples representing the number of passengers for each unique occupation</returns>
        public static List<(string, int)> GetOccupationsAndAmounts(this List<Passenger> passengers)
        {
            List<(string, int)> occupations = passengers
                .Select(p => p.Occupation)
                .GroupBy(x => x)
                .Select(x => (x.Key, x.Count()))
                .ToList();

            return occupations;
        }

        /// <summary>
        /// Returns a list of tuples representing the number of passengers in each age group
        /// </summary>
        /// <param name="passengers">A list of passengers</param>
        /// <param name="ageGroups">A list of tuples representing age group name and the age lower bound</param>
        /// <returns>A list of tuples representing the age groups name, the age lower bound, and the amount of people who fall into the desired range </returns>
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


    /// <summary>
    /// Represents a ship and its information
    /// </summary>
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
        // I don't think this class is needed (or a good idea), I'm keeping it just in case
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
