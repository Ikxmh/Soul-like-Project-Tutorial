using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimationHandler animationHandler;

        private void Awake()
        {
            animationHandler = GetComponentInChildren<AnimationHandler>();
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            animationHandler.PlayTargetAnimations(weapon.OH_Light_Attack_1, true);
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animationHandler.PlayTargetAnimations(weapon.OH_Heavy_Attack_1, true);
        }


    }
}

