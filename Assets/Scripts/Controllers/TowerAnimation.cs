using UnityEngine;
using UnityEngine.Assertions;
using Utils;

namespace Controllers
{
    public class TowerAnimation : MonoBehaviour
    {
        private static readonly int ChargeName = Animator.StringToHash("Charge");
        private static readonly int TriggerName = Animator.StringToHash("Trigger");

        public Animator animator;

        [Range(0, 1)]
        public float charge;

        public bool trigger;

        public void Start()
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }

            if (!animator)
            {
                animator = GetComponentInChildren<Animator>();
            }

            Assert.IsNotNull(animator);
        }

        public void Update()
        {
            animator.SetFloat(ChargeName, charge);

            if (trigger)
            {
                animator.ResetTrigger(TriggerName);
                animator.SetTrigger(TriggerName);

                trigger = false;
            }
        }

        public void SetCharge(GaugeState state)
        {
            if (state.max > 0)
            {
                float range = state.max - state.min;
                charge = range != 0 ? (state.value - state.min) / range : 1;
            }
            else
            {
                charge = 1;
            }

            charge = Mathf.Clamp(charge, 0, 1);
        }

        public void Trigger()
        {
            trigger = true;
        }
    }
}
