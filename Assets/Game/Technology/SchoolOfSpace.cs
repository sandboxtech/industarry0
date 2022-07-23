

using System;
using System.Collections.Generic;

namespace Weathering
{

    public class KnowledgeOfPlanetLander { }

    public class KnowledgeOfTerrainform { }

    [ConstructionCostBase(typeof(SchoolEquipment), 100, 10)]
    public class SchoolOfSpace : AbstractTechnologyCenter
    {
        public const long BaseCost = 1000;
        protected override long TechnologyPointMaxRevenue => BaseCost;
        protected override Type TechnologyPointType => typeof(CircuitBoardAdvanced);

        protected override List<(Type, long)> TechList => new List<(Type, long)> {

            (typeof(KnowledgeOfTerrainform), 1*BaseCost),
            (typeof(KnowledgeOfPlanetLander), 1*BaseCost),
            (typeof(LaunchSite), 2*BaseCost),
            //(typeof(SpaceElevator), 3*BaseCost),
            //(typeof(SpaceElevatorDest), 3*BaseCost),
            (typeof(Wardenclyffe), 3*BaseCost),
            (typeof(Torii), 3*BaseCost),
        };
    }
}
