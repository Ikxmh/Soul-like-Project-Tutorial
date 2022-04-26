using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IH
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        // variables for the weapons 
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel; 

        public void UnloadWeaponModel()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestory()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {

            UnloadWeaponAndDestory();
             
            if (weaponItem == null)
            {
                UnloadWeaponModel();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;

            if (model != null)
            {
               if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }

               // otherwise if there is no option of overriding 
               // then it is equal to the transform of the script
               else
                {
                    model.transform.parent.parent = transform;
                }

               // change the position to the player. 
               model.transform.localPosition = Vector3.zero;
               model.transform.localRotation = Quaternion.identity;
               model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
}

