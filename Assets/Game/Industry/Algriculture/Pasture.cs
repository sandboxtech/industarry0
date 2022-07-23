

using System;

namespace Weathering
{
    // 牛奶
    [Depend(typeof(Food))]
    public class Milk { }

    [ConstructionCostBase(typeof(Grain), 200, 20)]
    public class Pasture : AbstractFactoryStatic, IPassable
    {
        public bool Passable => true;

        protected override (Type, long) Out0 => (typeof(Milk), 3);
    }
}
