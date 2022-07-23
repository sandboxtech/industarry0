

using System;

namespace Weathering
{
    [Depend(typeof(DiscardableSolid))]
    public class SchoolEquipment { }


    [BindSpriteKey(typeof(Workshop))]
    [ConstructionCostBase(typeof(MachinePrimitive), 100)]
    public class WorkshopOfSchoolEquipment : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(SchoolEquipment), 1);

        protected override (Type, long) In_0 => (typeof(MachinePrimitive), 1);
    }
}
