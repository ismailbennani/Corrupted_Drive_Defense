using UnityEngine;
using Utils;

namespace Controllers
{
    public class ProcessorAnimation : MonoBehaviour
    {
        private static readonly int HealthName = Animator.StringToHash("Health");
        private static readonly int ChargeName = Animator.StringToHash("Charge");

        public Animator animator;


        [Range(0, 1)]
        public float health;

        [Range(0, 1)]
        public float charge;

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
                animator.SetFloat(HealthName, health);
                animator.SetFloat(ChargeName, charge);
            }
        }

        public void SetHealth(GaugeState state)
        {
            if (state.max > 0)
            {
                float range = state.max - state.min;
                health = range != 0 ? (state.value - state.min) / range : 1;
            }
            else
            {
                health = 1;
            }

            health = Mathf.Clamp(health, 0, 1);
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
    }
}
