using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class GameSpeedApi
    {
        private GameSpeed _speed;

        public GameSpeed GetSpeed()
        {
            return _speed;
        }

        public void CycleSpeed()
        {
            GameSpeed newSpeed;
            switch (_speed)
            {
                case GameSpeed.Normal:
                    newSpeed = GameSpeed.Fast;
                    break;
                case GameSpeed.Fast:
                    newSpeed = GameSpeed.VeryFast;
                    break;
                case GameSpeed.VeryFast:
                #if DEBUG
                    newSpeed = GameSpeed.Debug;
                    break;
                case GameSpeed.Debug:
                    newSpeed = GameSpeed.Normal;
                    break;
                #else
                    newSpeed = GameSpeed.Normal;
                    break;
                #endif
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetSpeed(newSpeed);
        }

        private void SetSpeed(GameSpeed speed)
        {
            Time.timeScale = GetTimeScale(speed);
            _speed = speed;
        }

        private static float GetTimeScale(GameSpeed speed)
        {
            return speed switch
            {
                GameSpeed.Normal => 1,
                GameSpeed.Fast => 2,
                GameSpeed.VeryFast => 4,
                #if DEBUG
                GameSpeed.Debug => 20,
                #endif
                _ => throw new ArgumentOutOfRangeException(nameof(speed), speed, null)
            };
        }
    }


    public enum GameSpeed
    {
        Normal,
        Fast,
        VeryFast,
        #if DEBUG
        Debug
        #endif
    }
}
