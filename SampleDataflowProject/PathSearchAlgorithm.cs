using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataflowProject
{
    public class PathSearchAlgorithm
    {
        public enum SearchingType { OnlyAirway, OnlyRailway }

        public static LinkedList<Road> ShortestPath(City departure, City arrival, SearchingType searchingType)
        {

            return DijkstraPath(departure, arrival);
        }

        public static LinkedList<Road> ClosestAirport(City city, bool isReverseSearch = false)
        {

        }

        private static LinkedList<Road> DijkstraPath(City departure, City arrival, Road.RoadType roadType)
        {
            Dictionary<City, int> cost = new Dictionary<City, int>();
            Dictionary<City, Road> prevRoad = new Dictionary<City, Road>();
            HashSet<City> alreadyCounted = new HashSet<City>();
            HashSet<City> counting = new HashSet<City>();

            counting.Add(departure);
            cost[departure] = 0;
            while (counting.Count > 0)
            {
                int minDist = counting.Min(city => cost[city]);
                City minCity = counting.First(city => (cost[city] == minDist));
                foreach (Road road in (roadType == Road.RoadType.Railway ? minCity.GetRailwayRoadsOut() : minCity.GetAirwayRoadsOut()))
                {
                    City neighbor = road.GetDestinationCity(); 
                    if (!alreadyCounted.Contains(neighbor))
                    {
                        if (counting.Contains(neighbor))
                        {
                            if (cost[neighbor] > cost[minCity] + road.GetCost())
                            {
                                cost[neighbor] = cost[minCity] + road.GetCost();
                                prevRoad[neighbor] = road;
                            }
                        }
                        else
                        {
                            cost[neighbor] = cost[minCity] + road.GetCost();
                            prevRoad[neighbor] = road;
                        }
                    }
                }
                counting.Remove(minCity);
                alreadyCounted.Add(minCity);

                if (minCity == arrival)
                {
                    LinkedList<Road> result = new LinkedList<Road>();
                    City currentCity = arrival;
                    while (prevRoad.ContainsKey(currentCity))
                    {
                        result.AddFirst(prevRoad[currentCity]);
                        currentCity = prevRoad[currentCity].GetDepartationCity();
                    }
                    return result;
                }
            }
            return null;
        }

        private class CityWrapper
        {
            public City city;
            public CityWrapper previous;
            public Road previousRoad;
            public int currentCost;

            public CityWrapper(City city)
            {
                this.city = city;
                previous = null;
                previousRoad = null;
                currentCost = int.MaxValue;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                return city == ((CityWrapper) obj).city;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                // TODO: write your implementation of GetHashCode() here
                throw new NotImplementedException();
                return base.GetHashCode();
            }
        }

    }
}
