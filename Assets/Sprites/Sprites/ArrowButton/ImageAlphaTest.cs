

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Weathering
{
    public class ImageAlphaTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public int arg;
        // public event UnityEngine.Events.UnityAction<int> OnChange;
        public void OnPointerDown(PointerEventData eventData) {
            // OnChange?.Invoke(arg);
            GameMenu.Ins.OnPointerChange(arg);
        }

        public void OnPointerUp(PointerEventData eventData) {
            // OnChange?.Invoke(-arg);
            GameMenu.Ins.OnPointerChange(-arg);
        }

        private void Awake() {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}
