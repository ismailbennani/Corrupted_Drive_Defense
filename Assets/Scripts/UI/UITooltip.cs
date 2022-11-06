﻿using TMPro;
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
            _rectTransform.anchoredPosition = Input.mousePosition;
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
