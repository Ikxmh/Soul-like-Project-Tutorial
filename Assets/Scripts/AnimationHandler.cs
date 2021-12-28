using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        int vertical;
        int horizontal;
        public bool canRotate;


        public void Initialize()
        {
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }


        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
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

            #endregion

            anim.SetFloat(vertical, vertValue, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, horizValue, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false; 
        }

    }
}
