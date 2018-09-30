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

            return DijkstraPath(departure, arrival, searchingType);
        }

        public static LinkedList<Road> ClosestAirport(City city, bool isReverseSearch = false)
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
                foreach (Road road in isReverseSearch ? minCity.GetRailwayRoadsIn() : minCity.GetRailwayRoadsOut())
                {
                    City neighbor = isReverseSearch ? road.GetDepartationCity() : road.GetDestinationCity();
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
                            counting.Add(neighbor);
                            cost[neighbor] = cost[minCity] + road.GetCost();
                            prevRoad[neighbor] = road;
                        }
                    }
                }
                counting.Remove(minCity);
                alreadyCounted.Add(minCity);

                if (minCity.HasAirport())
                {
                    LinkedList<Road> result = new LinkedList<Road>();
                    City currentCity = minCity;
                    while (prevRoad.ContainsKey(currentCity))
                    {
                        if (isReverseSearch)
                        {
                            result.AddLast(prevRoad[currentCity]);
                            currentCity = prevRoad[currentCity].GetDestinationCity();
                        }
                        else
                        {
                            result.AddFirst(prevRoad[currentCity]);
                            currentCity = prevRoad[currentCity].GetDepartationCity();
                        }
                        
                    }
                    return result;
                }
            }
            return null;
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
                foreach (Road road in (searchingType == SearchingType.OnlyRailway ? minCity.GetRailwayRoadsOut() : minCity.GetAirwayRoadsOut()))
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
                            counting.Add(neighbor);
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

    }
}
