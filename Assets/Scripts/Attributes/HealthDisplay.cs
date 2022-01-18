using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour 
    {
        Health health;
        [SerializeField] GameObject deathScreen;
        

        private void Awake() 
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() 
        {
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHP());
            if (health.getIsDead())
            {
                Die();
            }
        }

        private void Die()
        {
            deathScreen.SetActive(true);
        }
    }
}