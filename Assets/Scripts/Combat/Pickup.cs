using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour 
    {
        [SerializeField] GameObject equipment = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                EquipEquipment(other.gameObject);
            }
        }

        private void EquipEquipment(GameObject subject)
        {
            subject.GetComponent<Fighter>().EquipEquipment(equipment);
        
            Destroy(gameObject);
        }
    }
}