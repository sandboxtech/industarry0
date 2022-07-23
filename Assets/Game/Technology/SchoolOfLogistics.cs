

using System;
using System.Collections.Generic;

namespace Weathering
{
    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfLogistics : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;

        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(CombustionMotor);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(RoadForSolid), 0),
            (typeof(RoadAsBridge), 0),
            (typeof(RoadAsTunnel), 0),
            (typeof(RoadForFluid), 1*BaseCost),
            (typeof(RoadOfConcrete), 1*BaseCost),

            (typeof(TransportStationPort), 1*BaseCost),
            (typeof(TransportStationDestPort), 1*BaseCost),

            (typeof(RoadOfConcreteAsBridge), 2*BaseCost),
            (typeof(RoadAsTunnelAdvanced), 2*BaseCost),
            (typeof(RoadAsRailRoad), 2*BaseCost),
            (typeof(RoadLoaderOfRoadAsRailRoad), 2*BaseCost),


            (typeof(TransportStationAirport), 3*BaseCost),
            (typeof(TransportStationDestAirport), 3*BaseCost),
            (typeof(KnowledgeOfRunningFast), 3*BaseCost),
        };
    }
}
