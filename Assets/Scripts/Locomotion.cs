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
        PlayerManager playerManager;
        public Vector3 moveDirection;

        [HideInInspector]
        private Transform myTransform;
        [HideInInspector]
        public AnimationHandler animHandler;

        public new Rigidbody rigidbody;
        [SerializeField] private GameObject normalCamera;

        // setting up variables for ground & air 
        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceToStartFall = 1f;
        [SerializeField] float groundDistanceRayDistance = 0.2f;

        LayerMask ignoreForGroundCheck;
        public float inAirTimer; 


        // movement variables 
        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 5;
        [SerializeField] float rotationSpeed = 10;
        [SerializeField] float sprintSpeed = 7;
        [SerializeField] float fallingSpeed = 45; 
        [SerializeField] float walkingSpeed = 1; 

 
        // Start is called before the first frame update
        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animHandler = GetComponentInChildren<AnimationHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animHandler.Initialize();

            // start the player as grounded 
            playerManager.isGrounded = true;

            // layers to ignore 
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11); 

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
            if (inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return; 

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            if (inputHandler.sprintFlag & inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.moveAmount < 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                }
                
            }
  


            Vector3 projectVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectVelocity;

            animHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

            if (animHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        // determining the animations based on the character's movement 
        public void HandleRollingAndSprinting(float delta)
        {
            // can't roll when interacting 
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

        // preventing movement while falling 
        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position; 
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero; 
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir  = moveDirection;

            dir.Normalize();
            origin = origin + dir * groundDistanceRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceToStartFall, Color.red, 0.5f, false); 
            
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceToStartFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("in air for " + inAirTimer);
                        animHandler.PlayTargetAnimations("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animHandler.PlayTargetAnimations("Locomotion", false); 
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }

            }

            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if(playerManager.isInteracting == false)
                    {
                        animHandler.PlayTargetAnimations("Falling", true); 
                    }

                    Vector3 velocity = rigidbody.velocity;
                    rigidbody.velocity = velocity * (movementSpeed / 2); 
                    playerManager.isInAir = true;
                }
            }

            // 
            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }

        }

        #endregion

    }
}

