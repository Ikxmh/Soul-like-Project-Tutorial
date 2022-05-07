using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimationHandler animationHandler;
        InputHandler inputHandler;
        public string lastAttack; 

        private void Awake()
        {
            animationHandler = GetComponentInChildren<AnimationHandler>();
            inputHandler = GetComponentInChildren<InputHandler>();  
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animationHandler.anim.SetBool("CanDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animationHandler.PlayTargetAnimations(weapon.OH_Light_Attack_2, true);
                }
            }
            
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            animationHandler.PlayTargetAnimations(weapon.OH_Light_Attack_1, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animationHandler.PlayTargetAnimations(weapon.OH_Heavy_Attack_1, true);
            lastAttack= weapon.OH_Heavy_Attack_2;

        }


    }
}

