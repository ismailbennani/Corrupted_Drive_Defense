using UnityEngine;
using Utils;

namespace Controllers
{
    public class ProcessorAnimation: MonoBehaviour
    {
        private static readonly int HealthName = Animator.StringToHash("Health");
        private static readonly int TicksName = Animator.StringToHash("Ticks");

        public Animator animator;


        [Range(0, 1)]
        public float health = 0;
        
        [Range(0, 1)]
        public float ticks = 0;

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
                animator.SetFloat(TicksName, ticks);
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

        public void SetTicks(GaugeState state)
        {
            if (state.max > 0)
            {
                float range = state.max - state.min;
                ticks = range != 0 ? (state.value - state.min) / range : 1;
            }
            else
            {
                ticks = 1;
            }

            ticks = Mathf.Clamp(ticks, 0, 1);
        }
    }
}
