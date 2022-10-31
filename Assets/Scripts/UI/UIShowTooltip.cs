using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIShowTooltip: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string content;
        public UITooltip tooltip;

        private void Update()
        {
            tooltip.SetText(content);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!tooltip)
            {
                return;
            }
            
            tooltip.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!tooltip)
            {
                return;
            }
            
            tooltip.Hide();
        }
    }
}
