using UnityEngine.EventSystems;
using Utils.CustomComponents;

namespace UI
{
    public class UIClickInterceptor : MyMonoBehaviour, IPointerClickHandler
    {
        private void Start()
        {
            RequireGameManager();
        }


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
