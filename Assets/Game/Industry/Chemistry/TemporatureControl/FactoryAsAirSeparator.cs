

using System;

namespace Weathering
{

    // 氧气
    [Depend(typeof(DiscardableFluid))]
    public class Oxygen { }

    // 氮气
    [Depend(typeof(DiscardableFluid))]
    public class Nitrogen { }


    [BindSpriteKey(typeof(Factory))]
    public class FactoryAsAirSeparator : AbstractFactoryStatic
    {
        protected override (Type, long) In_0_Inventory => (typeof(Electricity), 10);

        protected override (Type, long) Out0 => (typeof(Nitrogen), 3);
        protected override (Type, long) Out1 => (typeof(Oxygen), 1);
    }
}
