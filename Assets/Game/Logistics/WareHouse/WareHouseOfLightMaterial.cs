

namespace Weathering
{
    [ConstructionCostBase(typeof(LightMaterial), 100)]
    [Concept]
    public class WareHouseOfLightMaterial : AbstractWareHouse
    {
        protected override long Capacity => 300000;
    }
}
