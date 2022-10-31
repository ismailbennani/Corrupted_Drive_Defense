using System;
using UnityEngine;

namespace GameComponents
{
    public class TowerAnimation : MonoBehaviour
    {
        private static readonly int ChargeName = Animator.StringToHash("Charge");
        private static readonly int TriggerName = Animator.StringToHash("Trigger");

        public Animator animator;

        [Range(0, 1)]
        public float charge = 0;

        public bool trigger = false;

        public void Start()
        {
            if (!animator)
            {
                animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
            }
        }

        public void Update()
        {
            if (animator)
            {
                animator.SetFloat(ChargeName, charge);

                if (trigger)
                {
                    animator.ResetTrigger(TriggerName);
                    animator.SetTrigger(TriggerName);

                    trigger = false;
                }
            }
        }

        public void SetCharge(float newCharge)
        {
            charge = newCharge;

            if (charge is < 0 or > 1)
            {
                charge = Mathf.Clamp(charge, 0, 1);
            }
        }

        public void Trigger()
        {
            trigger = true;
        }
    }
}
