using UnityEngine.EventSystems;
using Utils.CustomComponents;

namespace UI
{
    public class UIClickInterceptor : MyMonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!GameManager.Ready)
            {
                return;
            }

            GameManager.MouseInput.Click();
        }
    }
}
