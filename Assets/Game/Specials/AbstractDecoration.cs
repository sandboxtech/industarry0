

namespace Weathering
{
    public abstract class AbstractDecoration : StandardTile
    {
        public abstract override void OnTap();

        public override string SpriteKey {
            get {
                string result = UIItem.TryGetIconName(GetType());
                if (result == null) result = typeof(Pasture).Name;
                return result;
            }
        }

        public sealed override bool CanDestruct() => true;
    }
}
