using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{    
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float xpPoints = 0;

        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            xpPoints += experience;
            onExperienceGained();
        }

        public float GetXP()
        {
            return xpPoints;
        }

        public object CaptureState()
        {
            return xpPoints;
        }

        public void RestoreState(object state)
        {
            this.xpPoints = (float) state;
        }
    }
}