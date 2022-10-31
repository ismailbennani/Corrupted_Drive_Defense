using System;
using GameComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            _state.Value = value;

            UpdateComponents();
        }

        private void UpdateComponents()
        {
            if (foreground)
            {
                float ratio;
                if (_state.Max.HasValue)
                {
                    float range = _state.Max.Value - _state.Min;
                    ratio = range != 0 ? Mathf.Clamp((_state.Value - _state.Min) / range, 0, 1) : 1;
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
                if (_state.Max.HasValue && _state.Max.Value != 0)
                {
                    int actualMax = Mathf.FloorToInt(Mathf.Max(0, _state.Max.Value));
                    int value = Mathf.FloorToInt(Mathf.Clamp(_state.Value, _state.Min, _state.Max.Value));
                    str = $"{value} / {actualMax}";
                }
                else
                {
                    int value = Mathf.FloorToInt(Mathf.Max(_state.Min, _state.Value));
                    str = $"{value}";
                }

                text.text = str;
            }
        }
    }
}
