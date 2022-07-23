﻿

using System;

namespace Weathering
{
    [Depend(typeof(DiscardableSolid))]
    public class Book { }


    [ConstructionCostBase(typeof(Paper), 100)]
    public class WorkshopOfBook : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(Book), 1);

        protected override (Type, long) In_0 => (typeof(Paper), 1);
    }
}
