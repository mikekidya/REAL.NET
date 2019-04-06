using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataflowProject
{
    public class Road
    {
        public enum RoadType { Railway, Airway };

        public RoadType CurrentRoadType { get; private set; }
        public int Cost { get; private set; }
        public City DepartCity { get; private set; }
        public City DestinationCity { get; private set; }

        public Road(RoadType roadType, int cost, City departCity, City destinationCity)
        {
            CurrentRoadType = roadType;
            Cost = cost;
            DepartCity = departCity;
            DestinationCity = destinationCity;
        }
    }
}
