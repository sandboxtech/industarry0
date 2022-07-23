

using System;

namespace Weathering
{
    [Depend(typeof(DiscardableSolid))]
    public class BookWithNote { }


    [ConstructionCostBase(typeof(Paper), 100)]
    public class WorkshopOfBookWithNote : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(BookWithNote), 1);

        protected override (Type, long) In_0 => (typeof(Book), 1);
    }
}
