

using System;

namespace Weathering
{
    [ConstructionCostBase(typeof(ResetPointPop100), 1, 2)]
    public class ResetFarm : AbstractFactoryStatic, IPassable
    {
        public override string SpriteKeyRoad {
            get {
                int index = TileUtility.Calculate4x4RuleTileIndex(this, (tile, direction) => {
                    if (tile is Farm && tile is IRunnable runnable) {
                        return Running == runnable.Running;
                    }
                    return false;
                }
                );
                return $"{(Running ? "Farm" : "FarmGrowing")}_{index}";
            }
        }


        public bool Passable => true;

        protected override (Type, long) Out0 => (typeof(Grain), 6);
    }
}
