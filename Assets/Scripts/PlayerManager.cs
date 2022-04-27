using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class PlayerManager : MonoBehaviour
    {

        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        Locomotion playerLocomotion;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            cameraHandler = CameraHandler.singleton;
            playerLocomotion = GetComponent<Locomotion>(); 
        }

        // Update is called once per frame
        void Update()
        {
            isInteracting = anim.GetBool("IsInteracting");
           


            // Player Locomotion 
            Vector3 normalVector = Vector3.up;

            float delta = Time.deltaTime;

            
            inputHandler.TickInput(delta);

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection); 
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }


        // determining the flags.  
        private void LateUpdate()
        {
            // register one time 
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.rt_Input = false; 
            inputHandler.rb_Input = false; 


            // if in air then start the timer
            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }
    }

}