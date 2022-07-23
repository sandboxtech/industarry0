
using System;
using System.Collections.Generic;

namespace Weathering
{

    [ConstructionCostBase(typeof(Berry), 10, 20)]
    [Concept]
    public class BerryBushHydroponics : AbstractFactoryStatic, IPassable
    {
        protected override bool CanStoreSomething => true;
        protected override bool CanStoreOut0 => true;

        public bool Passable => true;


        public override string SpriteKeyRoad {
            get {
                return $"{(Running ? "BerryBushHydroponics" : "BerryBushHydroponics_Idle")}";
            }
        }

        public override string SpriteKey => null;

        public override string SpriteKeyHighLight => null;

        protected override (Type, long) In_0_Inventory => (typeof(Electricity), 1);

        protected override (Type, long) Out0 => (typeof(Berry), 6);
    }
}

