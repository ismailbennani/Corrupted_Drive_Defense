using TMPro;
using UnityEngine;

namespace UI
{
    public class UITooltip : MonoBehaviour
    {
        public TextMeshProUGUI text;

        private RectTransform _rectTransform;

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            _rectTransform.anchoredPosition = (Vector2)Input.mousePosition - new Vector2(Screen.currentResolution.width, Screen.currentResolution.height) / 2;
        }

        public void SetText(string str)
        {
            text.text = str;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
