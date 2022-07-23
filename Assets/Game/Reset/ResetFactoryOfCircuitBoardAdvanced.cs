

using System;

namespace Weathering
{
    [BindSpriteKey(typeof(Factory))]
    [ConstructionCostBase(typeof(ResetPointCircuit), 10, 2)]
    public class ResetFactoryOfCircuitBoardAdvanced : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 30);

        protected override (Type, long) Out0 => (typeof(CircuitBoardAdvanced), 1);

        protected override (Type, long) In_0 => (typeof(CircuitBoardIntegrated), 2);
        protected override (Type, long) In_1 => (typeof(LightMaterial), 1);
    }
}
