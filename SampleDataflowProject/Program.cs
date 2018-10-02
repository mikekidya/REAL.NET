using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace SampleDataflowProject
{
    class Program
    {
        

        static void Main(string[] args)
        {
            City a = new City("A");
            City b = new City("B");
            City c = new City("C");
            City d = new City("D");
            City e = new City("E");
            a.AddRailwayRoad(b, 1);
            a.AddRailwayRoad(e, 1000);
            b.AddRailwayRoad(d, 2);
            b.AddRailwayRoad(e, 10);
            c.AddRailwayRoad(b, 1);
            c.AddRailwayRoad(e, 1);
            d.AddRailwayRoad(c, 3);
            b.AddAirwayRoad(e, 100);
            d.AddAirwayRoad(e, 5);

            var start = new BroadcastBlock<Tuple<City, City>>(null);
            var railwayCounting = new TransformBlock<Tuple<City, City>, LinkedList<Road>>(arg =>
               PathSearchAlgorithm.ShortestPath(arg.Item1, arg.Item2, PathSearchAlgorithm.SearchingType.OnlyRailway));
            var startAirwayCounting = new BroadcastBlock<Tuple<City, City>>(null);
            var departCity = new TransformBlock<Tuple<City, City>, City>(arg => arg.Item1);
            var arrivalCity = new TransformBlock<Tuple<City, City>, City>(arg => arg.Item2);
            var departAirportAndPath = new TransformBlock<City, Tuple<City, LinkedList<Road>>>(arg => ClosestAirportAndPath(arg, false));
            var arrivalAirportAndPath = new TransformBlock<City, Tuple<City, LinkedList<Road>>>(arg => ClosestAirportAndPath(arg, true));
            var departAirport = new TransformBlock<Tuple<City, LinkedList<Road>>, City>(arg => arg.Item1);
            var arrivalAirport = new TransformBlock<Tuple<City, LinkedList<Road>>, City>(arg => arg.Item1);
            var departAirportPath = new TransformBlock<Tuple<City, LinkedList<Road>>, LinkedList<Road>>(arg => arg.Item2);
            var arrivalAirportPath = new TransformBlock<Tuple<City, LinkedList<Road>>, LinkedList<Road>>(arg => arg.Item2);
            var departAirportBroadcast = new BroadcastBlock<Tuple<City, LinkedList<Road>>>(null);
            var arrivalAirportBroadcast = new BroadcastBlock<Tuple<City, LinkedList<Road>>>(null);
            var airports = new JoinBlock<City, City>();
            var airwayCounting = new TransformBlock<Tuple<City, City>, LinkedList<Road>>(arg =>
            {
                var result = PathSearchAlgorithm.ShortestPath(arg.Item1, arg.Item2, PathSearchAlgorithm.SearchingType.OnlyAirway);
                return result;
            });
            var fullAirwayPathArgs = new JoinBlock<LinkedList<Road>, LinkedList<Road>, LinkedList<Road>>();
            var fullAirwayPath = new TransformBlock<Tuple<LinkedList<Road>, LinkedList<Road>, LinkedList<Road>>, LinkedList<Road>>(arg =>
            {
                return new LinkedList<Road>(arg.Item1.Concat(arg.Item2.Concat(arg.Item3)));
            });
                
            var printPath = new ActionBlock<LinkedList<Road>>(roads =>
            {
                foreach (Road road in roads)
                {
                    Console.Write(road.GetDepartationCity().GetName() + " -> ");
                }
                Console.WriteLine(roads.Last().GetDestinationCity().GetName());
            });

            start.LinkTo(railwayCounting);
            start.LinkTo(startAirwayCounting);

            railwayCounting.LinkTo(printPath);

            startAirwayCounting.LinkTo(departCity);
            startAirwayCounting.LinkTo(arrivalCity);

            departCity.LinkTo(departAirportAndPath);
            arrivalCity.LinkTo(arrivalAirportAndPath);

            departAirportAndPath.LinkTo(departAirportBroadcast);
            arrivalAirportAndPath.LinkTo(arrivalAirportBroadcast);

            departAirportBroadcast.LinkTo(departAirport);
            departAirportBroadcast.LinkTo(departAirportPath);
            arrivalAirportBroadcast.LinkTo(arrivalAirport);
            arrivalAirportBroadcast.LinkTo(arrivalAirportPath);

            departAirport.LinkTo(airports.Target1);
            arrivalAirport.LinkTo(airports.Target2);
            airports.LinkTo(airwayCounting);

            departAirportPath.LinkTo(fullAirwayPathArgs.Target1);
            airwayCounting.LinkTo(fullAirwayPathArgs.Target2);
            arrivalAirportPath.LinkTo(fullAirwayPathArgs.Target3);

            fullAirwayPathArgs.LinkTo(fullAirwayPath);

            fullAirwayPath.LinkTo(printPath);

            start.Post(new Tuple<City, City>(a, e));

            
            Console.ReadKey();
            


        }

        static Tuple<City, LinkedList<Road>> ClosestAirportAndPath(City city, bool isReverse)
        {
            LinkedList<Road> path = PathSearchAlgorithm.ClosestAirport(city, isReverse);
            City airport;
            if (isReverse)
            {
                airport = path.Count() > 0 ? path.First().GetDepartationCity() : city;
            }
            else
            {
                airport = path.Count() > 0 ? path.Last().GetDestinationCity() : city;
            }
            return new Tuple<City, LinkedList<Road>>(airport, path);
        }
    }
}
