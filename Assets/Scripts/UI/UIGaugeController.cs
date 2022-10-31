using System;
using GameComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIGaugeController : MonoBehaviour
    {
        public Image foreground;
        public TextMeshProUGUI text;

        private GaugeState _state;

        public void Set(GaugeState state)
        {
            _state = state;

            UpdateComponents();
        }

        public void Set(float value)
        {
            _state.value = value;

            UpdateComponents();
        }

        private void UpdateComponents()
        {
            if (foreground)
            {
                float ratio;
                if (_state.max > 0)
                {
                    float range = _state.max - _state.min;
                    ratio = range != 0 ? Mathf.Clamp((_state.value - _state.min) / range, 0, 1) : 1;
                }
                else
                {
                    ratio = 1;
                }
                
                foreground.fillAmount = ratio;
            }

            if (text)
            {
                string str;
                if (_state.max > 0)
                {
                    int max = Mathf.FloorToInt(_state.max);
                    int value = Mathf.FloorToInt(Mathf.Clamp(_state.value, _state.min, _state.max));
                    str = $"{value} / {max}";
                }
                else
                {
                    int value = Mathf.FloorToInt(Mathf.Max(_state.min, _state.value));
                    str = $"{value}";
                }

                text.text = str;
            }
        }
    }
}
