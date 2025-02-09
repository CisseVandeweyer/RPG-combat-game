using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;

        bool isDeath = false;


        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
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

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }


        public float GetPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetHealth());
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