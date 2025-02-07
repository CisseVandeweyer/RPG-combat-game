using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;

        bool isDeath = false;

        public bool IsDead()
        {
            return isDeath;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0f);
            if (health == 0)
            {
                Die();
            }
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