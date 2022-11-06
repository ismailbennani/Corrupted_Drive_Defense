using System;
using Managers;
using TMPro;
using UnityEngine.Assertions;
using Utils.CustomComponents;

namespace UI
{
    public class UISpeed : MyMonoBehaviour
    {
        public TextMeshProUGUI text;

        private void Start()
        {
            Assert.IsNotNull(text);

            RequireGameManager();
        }

        private void Update()
        {
            if (GameManager.GameSpeed == null)
            {
                return;
            }

            GameSpeed speed = GameManager.GameSpeed.GetSpeed();
            text.SetText(GetText(speed));
        }

        private string GetText(GameSpeed speed)
        {
            return speed switch
            {
                GameSpeed.Normal => "Speed x1",
                GameSpeed.Fast => "Speed x2",
                GameSpeed.VeryFast => "Speed x4",
                #if DEBUG
                GameSpeed.Debug => "DEBUG x20",
                #endif
                _ => throw new ArgumentOutOfRangeException(nameof(speed), speed, null)
            };
        }
    }
}
