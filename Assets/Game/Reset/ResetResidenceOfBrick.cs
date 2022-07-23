

using System;

namespace Weathering
{
    [BindSpriteKey(typeof(ResidenceOfBrick))]
    [ConstructionCostBase(typeof(ResetPointPop), 100, 2)]
    public class ResetResidenceOfBrick : AbstractFactoryStatic, IPassable
    {
        public bool Passable => false;

        protected override (Type, long) Out0_Inventory => (typeof(Worker), 4);
    }
}
