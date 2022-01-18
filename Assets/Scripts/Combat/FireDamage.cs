using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class FireDamage : MonoBehaviour
    {
        [SerializeField] float damage = 3f;
        [SerializeField] float coolDown = 1f;
        float timer = Mathf.Infinity;

        private void Update() 
        {
            timer += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.GetComponent<Health>() == null) return;
            if (timer > coolDown)
            {
                other.GetComponent<Health>().TakeDamage(gameObject, damage);
                timer = 0f;
            }
        }

        private void OnTriggerStay(Collider other) 
        {
            if (other.GetComponent<Health>() == null) return;
            if (timer > coolDown)
            {
                other.GetComponent<Health>().TakeDamage(gameObject, damage);
                timer = 0f;
            }
        }
    }
}