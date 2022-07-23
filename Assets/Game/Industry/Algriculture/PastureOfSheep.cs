

using System;

namespace Weathering
{

    [ConstructionCostBase(typeof(Grain), 200, 20)]
    public class PastureOfSheep : AbstractFactoryStatic, IPassable
    {
        public bool Passable => true;

        protected override (Type, long) Out0 => (typeof(Cotton), 1);
    }
}
