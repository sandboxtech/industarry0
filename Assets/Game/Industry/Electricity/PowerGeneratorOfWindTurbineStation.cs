

using System;
using UnityEngine;

namespace Weathering
{
    [ConstructionCostBase(typeof(WindTurbineComponent), 100, 50)]
    public class PowerGeneratorOfWindTurbineStation : AbstractFactoryStatic, IHasFrameAnimationOnSpriteKey
    {
        protected override (Type, long) Out0_Inventory => (typeof(Electricity), 20);

        public override string SpriteKey { 
            get {
                return $"{typeof(PowerGeneratorOfWindTurbineStation).Name}_{MapView.Ins.AnimationIndex % 2}";
            } 
        }

        public override string SpriteKeyHighLight => null;

        public int HasFrameAnimation => 1;
    }
}
