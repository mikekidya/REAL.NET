using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataflowProject
{
    public class City
    {
        public string Name { get; private set; }
        public bool HasAirport { get; private set; }
        public ICollection<Road> RailwayRoadsIn { get; private set; } = new LinkedList<Road>();
        public ICollection<Road> AirwayRoadsIn { get; private set; } = new LinkedList<Road>();
        public ICollection<Road> RailwayRoadsOut { get; private set; } = new LinkedList<Road>();
        public ICollection<Road> AirwayRoadsOut { get; private set; } = new LinkedList<Road>();

        public City(String name = "No name")
        {
            Name = name;
        }

        public void AddAirwayRoad(City destination, int cost)
        {
            var road = new Road(Road.RoadType.Airway, cost, this, destination);
            this.HasAirport = true;
            destination.HasAirport = true;
            this.AirwayRoadsOut.Add(road);
            destination.AirwayRoadsIn.Add(road);
        }

        public void AddRailwayRoad(City destination, int cost)
        {
            var road = new Road(Road.RoadType.Railway, cost, this, destination);
            RailwayRoadsOut.Add(road);
            destination.RailwayRoadsIn.Add(road);
        }
    }
}
