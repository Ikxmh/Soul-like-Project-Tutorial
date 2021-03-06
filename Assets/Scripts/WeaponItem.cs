using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{

    // Note:  instances of the type can be easily created and stored in the project as ".asset" files.
    [CreateAssetMenu(menuName = "Items/Weapon Item")]

    // inhert Item class
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed Attack Animations")]
        // light attacks
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        // heavy attacks 
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;
        
    }
}

