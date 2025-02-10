using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {

        [SerializeField] float regenerationPercentage = 70;
        float health = -1f;

        bool isDeath = false;


        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += Regeneratehealth;
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }


        public bool IsDead()
        {
            return isDeath;
        }

        public void TakeDamage(GameObject instagator, float damage)
        {
            health = Mathf.Max(health - damage, 0f);
            if (health == 0)
            {
                Die();
                AwardExperience(instagator);
            }
        }

        public float GetHealth()
        {
            return health;
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
            return 100 * (health / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Regeneratehealth()
        {
            float regenHealth = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            health = Mathf.Max(health + regenHealth);
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
            health = (float)state;

            if (health == 0)
            {
                Die();
            }
        }
    }
}