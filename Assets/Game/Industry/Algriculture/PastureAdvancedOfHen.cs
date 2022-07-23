

using System;

namespace Weathering
{
    [Depend(typeof(Creature))]
    public class Hen { }

    [ConstructionCostBase(typeof(StoneBrick), 200, 20)]
    public class PastureAdvancedOfHen : AbstractFactoryStatic, IPassable
    {
        public bool Passable => true;
        public override string SpriteKeyHighLight => null;

        protected override (Type, long) Out0 => (typeof(Egg), 3);
        protected override (Type, long) Out1 => (typeof(Hen), 1);
    }
}
