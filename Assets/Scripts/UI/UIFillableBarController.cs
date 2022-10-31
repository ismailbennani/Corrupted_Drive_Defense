using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFillableBarController : MonoBehaviour
    {
        public Image foreground;
        public TextMeshProUGUI text;

        private int _value;
        private int _max;

        public void SetValue(int value, int? max = null)
        {
            _value = value;
            if (max.HasValue)
            {
                _max = max.Value;
            }

            UpdateComponents();
        }

        private void UpdateComponents()
        {
            if (foreground)
            {
                float ratio = _max == 0 ? 0 : Mathf.Clamp(_value / _max, 0, 1);
                foreground.fillAmount = ratio;
            }

            if (text)
            {
                int actualMax = Mathf.Max(0, _max);
                int actualValue = Mathf.Clamp(_value, 0, actualMax);
                text.text = $"{actualValue} / {actualMax}";
            }
        }
    }
}
