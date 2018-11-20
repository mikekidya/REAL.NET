using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleDataflowProject;
using NSubstitute;
using NUnit.Framework;

namespace Dataflow.Tests
{
    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void ShortestRailwayPathTest()
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
            LinkedList<Road> path = (LinkedList<Road>)PathSearchAlgorithm.ShortestPath(a, e, PathSearchAlgorithm.SearchingType.OnlyRailway);
            Assert.AreEqual(path.Sum(_ => _.Cost), 7);
            Assert.AreEqual(path.First().DepartCity, a);
            Assert.AreEqual(path.First().DestinationCity, b);
            path.RemoveFirst();
            Assert.AreEqual(path.First().DepartCity, b);
            Assert.AreEqual(path.First().DestinationCity, d);
            path.RemoveFirst();
            Assert.AreEqual(path.First().DepartCity, d);
            Assert.AreEqual(path.First().DestinationCity, c);
            path.RemoveFirst();
            Assert.AreEqual(path.First().DepartCity, c);
            Assert.AreEqual(path.First().DestinationCity, e);
            path.RemoveFirst();
            Assert.AreEqual(path.Count(), 0);
        }

        [Test]
        public void ShortestAirwayPathTest()
        {
            var a = new City("A");
            var b = new City("B");
            var c = new City("C");
            var d = new City("D");
            var e = new City("E");
            a.AddAirwayRoad(b, 1);
            a.AddAirwayRoad(e, 1000);
            b.AddAirwayRoad(d, 2);
            b.AddAirwayRoad(e, 10);
            c.AddAirwayRoad(b, 1);
            c.AddAirwayRoad(e, 1);
            d.AddAirwayRoad(c, 3);
            LinkedList<Road> path = (LinkedList<Road>)PathSearchAlgorithm.ShortestPath(a, e, PathSearchAlgorithm.SearchingType.OnlyAirway);
            Assert.AreEqual(path.Sum(_ => _.Cost), 7);
            Assert.AreEqual(path.First().DepartCity, a);
            Assert.AreEqual(path.First().DestinationCity, b);
            path.RemoveFirst();
            Assert.AreEqual(path.First().DepartCity, b);
            Assert.AreEqual(path.First().DestinationCity, d);
            path.RemoveFirst();
            Assert.AreEqual(path.First().DepartCity, d);
            Assert.AreEqual(path.First().DestinationCity, c);
            path.RemoveFirst();
            Assert.AreEqual(path.First().DepartCity, c);
            Assert.AreEqual(path.First().DestinationCity, e);
            path.RemoveFirst();
            Assert.AreEqual(path.Count(), 0);
        }

        [Test]
        public void ClosestAirportTest()
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
            a.AddAirwayRoad(e, 100);
            d.AddAirwayRoad(e, 5);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(a).Count(), 0);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(b).Last().DestinationCity, d);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(c).Last().DestinationCity, e);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(d).Count(), 0);
        }

        [Test]
        public void OnlyAirportsTest()
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
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(a).Last().DestinationCity, b);
            Assert.AreEqual(PathSearchAlgorithm.ShortestPath(b, e, PathSearchAlgorithm.SearchingType.OnlyAirway).Last().DestinationCity, e);
        }
    }
}
