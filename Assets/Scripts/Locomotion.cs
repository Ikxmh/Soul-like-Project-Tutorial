using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IH

{
    public class Locomotion : MonoBehaviour
    {
        // variables for getting the character to move around 

        Transform cameraObject;
        InputHandler inputHandler;
        private Vector3 moveDirection;

        [HideInInspector]
        private Transform myTransform;
        [HideInInspector]
        public AnimationHandler animHandler;

        public new Rigidbody rigidbody;
        [SerializeField] private GameObject normalCamera;

        [Header("Stats")]
        [SerializeField] float movementSpeed = 5;
        [SerializeField] float rotationSpeed = 10;

 
        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animHandler = GetComponentInChildren<AnimationHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animHandler.Initialize();

        }

        public void Update()
        {

            Vector3 normalVector = Vector3.up;

            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);

            HandleMovement(delta);
            HandleRollingAndSprinting(delta); 

        }


        #region Movement
        private Vector3 normalVector;
        private Vector3 targetPosition;

        // handing the character's rotation
        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;


            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;


            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation; 

        }

        // handling the character's movement

        public void HandleMovement(float delta)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            moveDirection *= speed;


            Vector3 projectVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectVelocity;

            animHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        // determining the animations based on the character's movement 
        public void HandleRollingAndSprinting(float delta)
        {
            if (animHandler.anim.GetBool("IsInteracting"))
                return;

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animHandler.PlayTargetAnimations("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation; 
                }
                else
                {
                    animHandler.PlayTargetAnimations("Backstep", true); 
                }
            }
        }

        #endregion

    }
}

