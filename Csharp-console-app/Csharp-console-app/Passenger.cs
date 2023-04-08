using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_console_app
{
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

        public override string ToString()
        {
            return $"{this.LastName} {this.FirstName} aged {this.Age}";
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
            // instead of typing
            // p.Ship.ShipID == ship.ShipID
            // I could implement ship equality
        }
    
        public static List<(string, int)> GetOccupations(this List<Passenger> passengers)
        {
            List<(string, int)> occupations = passengers
                .Select(p => p.Occupation)
                .GroupBy(x => x)
                .Select(x => (x.Key, x.Count()))
                .ToList();

            return occupations;
        }
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
