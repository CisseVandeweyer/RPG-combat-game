using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {

        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        LazyValue<float> health;

        bool isDeath = false;


        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }


        private void Start()
        {
            health.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += Regeneratehealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= Regeneratehealth;
        }


        public bool IsDead()
        {
            return isDeath;
        }

        public void TakeDamage(GameObject instagator, float damage)
        {
            health.value = Mathf.Max(health.value - damage, 0f);
            if (health.value == 0)
            {
                Die();
                AwardExperience(instagator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealth()
        {
            return health.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.Health));
        }


        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        private void Regeneratehealth()
        {
            float regenHealth = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            health.value = Mathf.Max(health.value + regenHealth);
        }

        private void Die()
        {
            if (isDeath) return;

            isDeath = true;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;

            if (health.value == 0)
            {
                Die();
            }
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
    }
}