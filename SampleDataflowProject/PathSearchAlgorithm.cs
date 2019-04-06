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

        public static ICollection<Road> ShortestPath(City departure, City arrival, SearchingType searchingType)
        {

            return DijkstraPath(departure, arrival, searchingType);
        }

        public static ICollection<Road> ClosestAirport(City city, bool isReverseSearch = false)
        {
            Dictionary<City, int> cost = new Dictionary<City, int>();
            Dictionary<City, Road> prevRoad = new Dictionary<City, Road>();
            HashSet<City> alreadyCounted = new HashSet<City>();
            HashSet<City> counting = new HashSet<City>();

            counting.Add(city);
            cost[city] = 0;
            while (counting.Count > 0)
            {
                int minDist = counting.Min(_ => cost[_]);
                City minCity = counting.First(_ => (cost[_] == minDist));
                foreach (Road road in isReverseSearch ? minCity.RailwayRoadsIn : minCity.RailwayRoadsOut)
                {
                    City neighbor = isReverseSearch ? road.DepartCity : road.DestinationCity;
                    if (!alreadyCounted.Contains(neighbor))
                    {
                        if (counting.Contains(neighbor))
                        {
                            if (cost[neighbor] > cost[minCity] + road.Cost)
                            {
                                cost[neighbor] = cost[minCity] + road.Cost;
                                prevRoad[neighbor] = road;
                            }
                        }
                        else
                        {
                            counting.Add(neighbor);
                            cost[neighbor] = cost[minCity] + road.Cost;
                            prevRoad[neighbor] = road;
                        }
                    }
                }
                counting.Remove(minCity);
                alreadyCounted.Add(minCity);

                if (minCity.HasAirport)
                {
                    LinkedList<Road> result = new LinkedList<Road>();
                    City currentCity = minCity;
                    while (prevRoad.ContainsKey(currentCity))
                    {
                        if (isReverseSearch)
                        {
                            result.AddLast(prevRoad[currentCity]);
                            currentCity = prevRoad[currentCity].DestinationCity;
                        }
                        else
                        {
                            result.AddFirst(prevRoad[currentCity]);
                            currentCity = prevRoad[currentCity].DepartCity;
                        }
                        
                    }
                    return result;
                }
            }
            return new LinkedList<Road>();
        }
    

        private static LinkedList<Road> DijkstraPath(City departure, City arrival, SearchingType searchingType)
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
                foreach (Road road in (searchingType == SearchingType.OnlyRailway ? minCity.RailwayRoadsOut : minCity.AirwayRoadsOut))
                {
                    City neighbor = road.DestinationCity; 
                    if (!alreadyCounted.Contains(neighbor))
                    {
                        if (counting.Contains(neighbor))
                        {
                            if (cost[neighbor] > cost[minCity] + road.Cost)
                            {
                                cost[neighbor] = cost[minCity] + road.Cost;
                                prevRoad[neighbor] = road;
                            }
                        }
                        else
                        {
                            counting.Add(neighbor);
                            cost[neighbor] = cost[minCity] + road.Cost;
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
                        currentCity = prevRoad[currentCity].DepartCity;
                    }
                    return result;
                }
            }
            return new LinkedList<Road>();
        }

    }
}
