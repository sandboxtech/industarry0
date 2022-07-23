

using System;

namespace Weathering
{
    [Depend(typeof(DiscardableSolid))]
    public class BookWithNoteAdvanced { }


    [ConstructionCostBase(typeof(Paper), 100)]
    public class WorkshopOfBookWithNoteAdvanced : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(BookWithNoteAdvanced), 1);

        protected override (Type, long) In_0 => (typeof(Book), 2);
        protected override (Type, long) In_1 => (typeof(SchoolEquipment), 1);
    }
}
