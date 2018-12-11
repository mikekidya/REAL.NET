using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace SampleDataflowProject
{
    class AutomaticallyWrittenPrinter : ISamplePathPrinter
    {
        public void printPath()
        {
            var a = new City("A");
            var b = new City("B");
            var c = new City("C");
            var d = new City("D");
            var e = new City("E");
            a.AddRailwayRoad(b, 1);
            a.AddRailwayRoad(e, 1000);
            b.AddRailwayRoad(d, 2);
            b.AddRailwayRoad(e, 10);
            c.AddRailwayRoad(b, 1);
            c.AddRailwayRoad(e, 1);
            d.AddRailwayRoad(c, 3);
            b.AddAirwayRoad(e, 100);
            d.AddAirwayRoad(e, 5);

            var printPath = new ActionBlock<ICollection<Road>>(roads =>
            {
                foreach (Road road in roads)
                {
                    Console.Write(road.DepartCity.Name + " -> ");
                }
                Console.WriteLine(roads.Last().DestinationCity.Name);
            });
            var fullAirwayPath = new TransformBlock<Tuple<ICollection<Road>, ICollection<Road>, ICollection<Road>>, ICollection<Road>>(arg =>
            {
                return new LinkedList<Road>(arg.Item1.Concat(arg.Item2.Concat(arg.Item3)));
            });

            var airwayCount = new TransformBlock<Tuple<City, City>, ICollection<Road>>(arg =>
            {
                var result = PathSearchAlgorithm.ShortestPath(arg.Item1, arg.Item2, PathSearchAlgorithm.SearchingType.OnlyAirway);
                return result;
            });
            var arrivalAirportPath = new TransformBlock<Tuple<City, ICollection<Road>>, ICollection<Road>>(arg => arg.Item2);
            var arrivalAirport = new TransformBlock<Tuple<City, ICollection<Road>>, City>(arg => arg.Item1);
            var departAirportPath = new TransformBlock<Tuple<City, ICollection<Road>>, ICollection<Road>>(arg => arg.Item2);
            var departAirport = new TransformBlock<Tuple<City, ICollection<Road>>, City>(arg => arg.Item1);
            var arrivalAirportAndPath = new TransformBlock<City, Tuple<City, ICollection<Road>>>(arg => ClosestAirportAndPath(arg, true));
            var departAirportAndPath = new TransformBlock<City, Tuple<City, ICollection<Road>>>(arg => ClosestAirportAndPath(arg, false));
            var arrivalCity = new TransformBlock<Tuple<City, City>, City>(arg => arg.Item2);
            var departCity = new TransformBlock<Tuple<City, City>, City>(arg => arg.Item1);
            var railwayCounting = new TransformBlock<Tuple<City, City>, ICollection<Road>>(arg =>
               PathSearchAlgorithm.ShortestPath(arg.Item1, arg.Item2, PathSearchAlgorithm.SearchingType.OnlyRailway));
            var start = new BroadcastBlock<Tuple<City, City>>(null);

            var fullAirwayPathJoinBlock = new JoinBlock<ICollection<Road>, ICollection<Road>, ICollection<Road>>();
            var airwayCountJoinBlock = new JoinBlock<City, City>();
            var arrivalAirportAndPathBroadcastBlock = new BroadcastBlock<Tuple<City, ICollection<Road>>>(null);
            var departAirportAndPathBroadcastBlock = new BroadcastBlock<Tuple<City, ICollection<Road>>>(null);
            var startBroadcastBlock = new BroadcastBlock<Tuple<City, City>>(null);

            fullAirwayPathJoinBlock.LinkTo(fullAirwayPath);
            fullAirwayPath.LinkTo(printPath);
            airwayCountJoinBlock.LinkTo(airwayCount);
            airwayCount.LinkTo(fullAirwayPathJoinBlock.Target2);
            arrivalAirportPath.LinkTo(fullAirwayPathJoinBlock.Target3);
            arrivalAirport.LinkTo(airwayCountJoinBlock.Target2);
            departAirportPath.LinkTo(fullAirwayPathJoinBlock.Target1);
            departAirport.LinkTo(airwayCountJoinBlock.Target1);
            arrivalAirportAndPath.LinkTo(arrivalAirportAndPathBroadcastBlock);
            arrivalAirportAndPathBroadcastBlock.LinkTo(arrivalAirportPath);
            arrivalAirportAndPathBroadcastBlock.LinkTo(arrivalAirport);
            departAirportAndPath.LinkTo(departAirportAndPathBroadcastBlock);
            departAirportAndPathBroadcastBlock.LinkTo(departAirportPath);
            departAirportAndPathBroadcastBlock.LinkTo(departAirport);
            arrivalCity.LinkTo(arrivalAirportAndPath);
            departCity.LinkTo(departAirportAndPath);
            railwayCounting.LinkTo(printPath);
            start.LinkTo(startBroadcastBlock);
            startBroadcastBlock.LinkTo(arrivalCity);
            startBroadcastBlock.LinkTo(departCity);
            startBroadcastBlock.LinkTo(railwayCounting);

            start.Post(new Tuple<City, City>(a, e));


        }
        private Tuple<City, ICollection<Road>> ClosestAirportAndPath(City city, bool isReverse)
        {
            ICollection<Road> path = PathSearchAlgorithm.ClosestAirport(city, isReverse);
            City airport;
            if (isReverse)
            {
                airport = path.Count() > 0 ? path.First().DepartCity : city;
            }
            else
            {
                airport = path.Count() > 0 ? path.Last().DestinationCity : city;
            }
            return new Tuple<City, ICollection<Road>>(airport, path);
        }
    }
}
