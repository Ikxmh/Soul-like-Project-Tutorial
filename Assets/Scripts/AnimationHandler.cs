using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class AnimationHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        // setting up the variables 
        public Animator anim;
        InputHandler inputHandler;
        Locomotion playerLocomotion;

        int vertical;
        int horizontal;
        public bool canRotate;

        // initializing before player starts the game. 
        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<Locomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }


        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float vertValue = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                vertValue = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                vertValue = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                vertValue = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                vertValue = -1;
            }
            else
            {
                vertValue = 0;
            }
            #endregion

            #region Horizontal
            float horizValue = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                horizValue = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                horizValue = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                horizValue = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                horizValue = -1;
            }
            else
            {
                horizValue = 0;
            }

            if (isSprinting)
            {
                vertValue = 1; 
                
                horizValue = horizontalMovement; 
            }

            #endregion

            anim.SetFloat(vertical, vertValue, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, horizValue, 0.1f, Time.deltaTime);
        }


        public void PlayTargetAnimations(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("IsInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false; 
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity; 

        }

    }
}
