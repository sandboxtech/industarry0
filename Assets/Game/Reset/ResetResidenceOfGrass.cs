

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(ResetPointPop), 10, 2)]
    public class ResetResidenceOfGrass : AbstractFactoryStatic, IPassable
    {
        public bool Passable => false;

        protected override (Type, long) Out0_Inventory => (typeof(Worker), 1);
    }
}
