using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat 
{
    public class DropItem : MonoBehaviour
    {
        [SerializeField] Pickup item = null;
        [SerializeField] GameObject target = null;

        Weapon currentWeapon;
        Health health;

        private void Start() 
        {
            health = GetComponent<Health>();
            currentWeapon = GetComponent<Fighter>().GetCurrentWeapon();
        }

        private void Update() 
        {
            if (health.getIsDead())
            {
                DropEquipment();
                this.enabled = false;
            }
        }

        public void DropEquipment()
        {
            if (item == null) return;
            Instantiate(item, target.transform, false);
            Destroy(currentWeapon.gameObject);
        }
    }
}
