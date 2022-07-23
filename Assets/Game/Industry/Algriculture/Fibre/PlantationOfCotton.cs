

using System;

namespace Weathering
{
    [Depend(typeof(Food))]
    public class Cotton { }


    [ConstructionCostBase(typeof(Grain), 10, 20)]
    [Concept]
    public class PlantationOfCotton : AbstractFactoryStatic, IPassable
    {
        protected override bool CanStoreSomething => false;
        protected override bool CanStoreOut0 => false;

        public bool Passable => true;

        public override string SpriteKeyRoad {
            get {
                int index = TileUtility.Calculate4x4RuleTileIndex(this, (tile, direction) => {
                    if (tile is PlantationOfCotton && tile is IRunnable runnable) {
                        return Running == runnable.Running;
                    }
                    return false;
                }
                );
                return $"{(Running ? "PlantationOfCotton" : "PlantationOfCottonGrowing")}_{index}";
            }
        }
        public override string SpriteKey => null;
        public override string SpriteKeyHighLight => null;

        protected override (Type, long) In_0_Inventory => (typeof(Worker), 1);

        protected override (Type, long) Out0 => (typeof(Cotton), 1);
    }
}
