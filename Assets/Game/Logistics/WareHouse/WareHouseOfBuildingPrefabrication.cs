

namespace Weathering
{
    [ConstructionCostBase(typeof(BuildingPrefabrication), 100)]
    [Concept]
    public class WareHouseOfBuildingPrefabrication : AbstractWareHouse
    {
        protected override long Capacity => 100000;
    }
}
