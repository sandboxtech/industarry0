

using System;

namespace Weathering
{
    [Depend(typeof(Creature))]
    public class Cow { }

    [ConstructionCostBase(typeof(StoneBrick), 200, 20)]
    public class PastureAdvancedOfCow : AbstractFactoryStatic, IPassable
    {
        public bool Passable => true;
        public override string SpriteKeyHighLight => null;

        protected override (Type, long) Out0 => (typeof(Milk), 3);
        protected override (Type, long) Out1 => (typeof(Cow), 1);
    }
}
