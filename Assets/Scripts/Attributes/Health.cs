using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes 
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] bool isHorse = false;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        bool isDead = false;
        BaseStats baseStats;

        private void Awake() 
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            if (isHorse) return 1f;
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() 
        {
            healthPoints.ForceInit();
        }

        private void OnEnable() 
        {
            if (isHorse) return;
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() 
        {
            if (isHorse) return;
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool getIsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamage.Invoke(damage);

            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHP());
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHP()
        {
            if (isHorse) return 1;
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            if (isHorse) return 1f;
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;
            
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}