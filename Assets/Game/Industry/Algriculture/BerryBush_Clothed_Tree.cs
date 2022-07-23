

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(Berry), 100, 10)]
    public class BerryBush_Clothed_Tree : AbstractFactoryStatic
    {
        protected override bool CanStoreSomething => true;
        protected override bool CanStoreOut0 => true;

        public bool Passable => true;

        protected override (Type, long) Out0 => (typeof(Berry), 3);
    }
}
