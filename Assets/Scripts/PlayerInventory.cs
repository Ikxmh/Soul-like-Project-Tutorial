using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class PlayerInventory : MonoBehaviour
    {
        // get the information from weapons and slots 
        WeaponSlotManager weaponSlotManager; 

        // determine what weapons are in our hands 
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;


        private void Awake()
        {
            // placing on top where input handler and player manager is 
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon,false);

            // will be true because it is on the left slot 
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon,true);
        }
    }
}

