using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataflowProject
{
    public class City
    {
        private readonly String name;
        private bool isHasAirport = false;
        private LinkedList<Road> railwayRoads;
        private LinkedList<Road> airwayRoads;

        public City(String name = "No name")
        {
            this.name = name;
            railwayRoads = new LinkedList<Road>();
        }

        public void AddAirwayRoad(City destination, int cost)
        {
            Road road = new Road(Road.RoadType.Airway, cost, this, destination);
            if (!HasAirport())
            {
                isHasAirport = true;
                airwayRoads = new LinkedList<Road>();
            }
            airwayRoads.AddLast(road);
        }

        public void AddRailwayRoad(City destination, int cost)
        {
            railwayRoads.AddLast(new Road(Road.RoadType.Railway, cost, this, destination));
        }

        public bool HasAirport()
        {
            return isHasAirport;
        }

        public LinkedList<Road> GetAirwayRoads()
        {
            return airwayRoads;
        }

        public LinkedList<Road> GetRailwayRoads()
        {
            return railwayRoads;
        }

        public String GetName()
        {
            return name;
        }
    }
}
