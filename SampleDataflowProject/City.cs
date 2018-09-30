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
        private LinkedList<Road> railwayRoadsIn;
        private LinkedList<Road> airwayRoadsIn;
        private LinkedList<Road> railwayRoadsOut;
        private LinkedList<Road> airwayRoadsOut;

        public City(String name = "No name")
        {
            this.name = name;
            railwayRoadsIn = new LinkedList<Road>();
            railwayRoadsOut = new LinkedList<Road>();
        }

        public void AddAirwayRoad(City destination, int cost)
        {
            Road road = new Road(Road.RoadType.Airway, cost, this, destination);
            if (!HasAirport())
            {
                isHasAirport = true;
                airwayRoadsIn = new LinkedList<Road>();
            }
            if (!destination.HasAirport())
            {
                destination.isHasAirport = true;
                destination.airwayRoadsOut = new LinkedList<Road>();
            }
            this.airwayRoadsOut.AddLast(road);
            destination.airwayRoadsIn.AddLast(road);
        }

        public void AddRailwayRoad(City destination, int cost)
        {
            Road road = new Road(Road.RoadType.Railway, cost, this, destination);
            railwayRoadsOut.AddLast(road);
            destination.railwayRoadsIn.AddLast(road);
        }

        public bool HasAirport()
        {
            return isHasAirport;
        }

        public LinkedList<Road> GetAirwayRoadsIn()
        {
            return airwayRoadsIn;
        }

        public LinkedList<Road> GetRailwayRoadsIn()
        {
            return railwayRoadsIn;
        }

        public LinkedList<Road> GetAirwayRoadsOut()
        {
            return airwayRoadsOut;
        }

        public LinkedList<Road> GetRailwayRoadsOut()
        {
            return railwayRoadsOut;
        }

        public String GetName()
        {
            return name;
        }
    }
}
