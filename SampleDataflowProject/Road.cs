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

        private readonly RoadType roadType;
        private readonly int cost;
        private readonly City departCity;
        private readonly City destinationCity;

        public Road(RoadType roadType, int cost, City departCity, City destinationCity)
        {
            this.roadType = roadType;
            this.cost = cost;
            this.departCity = departCity;
            this.destinationCity = destinationCity;
        }

        public RoadType GetRoadType()
        {
            return roadType;
        }

        public City GetDepartationCity()
        {
            return departCity;
        }

        public City GetDestinationCity()
        {
            return destinationCity;
        }

        public int GetCost()
        {
            return cost;
        }
    }
}
