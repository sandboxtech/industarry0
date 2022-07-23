
using System;
using System.Collections.Generic;

namespace Weathering
{

    [ConstructionCostBase(typeof(Grain), 10, 20)]
    [Concept]
    public class FarmHydroponics : AbstractFactoryStatic, IPassable
    {
        protected override bool CanStoreSomething => true;
        protected override bool CanStoreOut0 => true;

        public bool Passable => true;


        public override string SpriteKeyRoad {
            get {
                return $"{(Running ? "FarmHydroponics" : "FarmHydroponics_Idle")}";
            }
        }

        public override string SpriteKey => null;
        public override string SpriteKeyHighLight => null;

        protected override (Type, long) In_0_Inventory => (typeof(Electricity), 3);

        protected override (Type, long) Out0 => (typeof(Grain), 12);
    }
}

