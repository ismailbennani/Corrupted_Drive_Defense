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

        public void SetCharge(GaugeState state)
        {
            if (state.Max.HasValue)
            {
                float range = state.Max.Value - state.Min;
                charge = range != 0 ? (state.Value - state.Min) / range : 1;
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