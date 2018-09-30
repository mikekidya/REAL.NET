using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleDataflowProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataflowProject.Tests
{
    [TestClass()]
    public class PathSearchAlgorithmTests
    {
        [TestMethod()]
        public void ShortestRailwayPathTest()
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
            LinkedList<Road> path = PathSearchAlgorithm.ShortestPath(a, e, PathSearchAlgorithm.SearchingType.OnlyRailway);
            Assert.AreEqual(path.Sum(_ => _.GetCost()), 7);
            Assert.AreEqual(path.First().GetDepartationCity(), a);
            Assert.AreEqual(path.First().GetDestinationCity(), b);
            path.RemoveFirst();
            Assert.AreEqual(path.First().GetDepartationCity(), b);
            Assert.AreEqual(path.First().GetDestinationCity(), d);
            path.RemoveFirst();
            Assert.AreEqual(path.First().GetDepartationCity(), d);
            Assert.AreEqual(path.First().GetDestinationCity(), c);
            path.RemoveFirst();
            Assert.AreEqual(path.First().GetDepartationCity(), c);
            Assert.AreEqual(path.First().GetDestinationCity(), e);
            path.RemoveFirst();
            Assert.AreEqual(path.Count(), 0);
        }

        [TestMethod()]
        public void ShortestAirwayPathTest()
        {
            City a = new City("A");
            City b = new City("B");
            City c = new City("C");
            City d = new City("D");
            City e = new City("E");
            a.AddAirwayRoad(b, 1);
            a.AddAirwayRoad(e, 1000);
            b.AddAirwayRoad(d, 2);
            b.AddAirwayRoad(e, 10);
            c.AddAirwayRoad(b, 1);
            c.AddAirwayRoad(e, 1);
            d.AddAirwayRoad(c, 3);
            LinkedList<Road> path = PathSearchAlgorithm.ShortestPath(a, e, PathSearchAlgorithm.SearchingType.OnlyAirway);
            Assert.AreEqual(path.Sum(_ => _.GetCost()), 7);
            Assert.AreEqual(path.First().GetDepartationCity(), a);
            Assert.AreEqual(path.First().GetDestinationCity(), b);
            path.RemoveFirst();
            Assert.AreEqual(path.First().GetDepartationCity(), b);
            Assert.AreEqual(path.First().GetDestinationCity(), d);
            path.RemoveFirst();
            Assert.AreEqual(path.First().GetDepartationCity(), d);
            Assert.AreEqual(path.First().GetDestinationCity(), c);
            path.RemoveFirst();
            Assert.AreEqual(path.First().GetDepartationCity(), c);
            Assert.AreEqual(path.First().GetDestinationCity(), e);
            path.RemoveFirst();
            Assert.AreEqual(path.Count(), 0);
        }

        [TestMethod()]
        public void ClosestAirportTest()
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
            a.AddAirwayRoad(e, 100);
            d.AddAirwayRoad(e, 5);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(a).Count(), 0);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(b).Last().GetDestinationCity(), d);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(c).Last().GetDestinationCity(), e);
            Assert.AreEqual(PathSearchAlgorithm.ClosestAirport(d).Count(), 0);
        }
    }
}