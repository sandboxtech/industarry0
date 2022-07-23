

using System;

namespace Weathering
{
    [Depend(typeof(Creature))]
    public class Pig { }

    [ConstructionCostBase(typeof(StoneBrick), 200, 20)]
    public class PastureAdvancedOfPig : AbstractFactoryStatic, IPassable
    {
        public bool Passable => true;
        public override string SpriteKeyHighLight => null;

        protected override (Type, long) Out1 => (typeof(Pig), 1);
    }
}
