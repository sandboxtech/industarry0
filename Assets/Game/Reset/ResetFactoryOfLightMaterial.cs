

using System;

namespace Weathering
{

    [BindSpriteKey(typeof(Factory))]
    [ConstructionCostBase(typeof(ResetPointLightMaterial), 10, 2)]
    public class ResetFactoryOfLightMaterial : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Worker), 2);
        protected override (Type, long) In_1_Inventory => (typeof(Electricity), 10);

        protected override (Type, long) Out0 => (typeof(LightMaterial), 1);
    }
}
