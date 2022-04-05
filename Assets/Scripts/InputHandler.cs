using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class InputHandler : MonoBehaviour
    {
        // variables for getting the inputs 

        public float horizontal;
        public float vertical;
        public float moveAmount;
        [SerializeField] private float mouseX;
        [SerializeField] private float mouseY;

        private PlayerControls inputActions;
        private CameraHandler cameraHandler;

        private Vector2 movementinput;
        private Vector2 cameraInput;


        public bool b_Input;

        public bool rollFlag;
        public bool sprintFlag;
        public float RollInputTimer; 
        public bool isInteracting; 


        private void Start()
        {
            cameraHandler = CameraHandler.singleton; 
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime; 

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }



        // if the input action == null then read the values else enable the player's input 
        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementinput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        // determining the inputs 
        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta); 
        }


        // getting that character to move 
        public void MoveInput(float delta)
        {
            horizontal = movementinput.x;
            vertical = movementinput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }


        private void HandleRollInput(float delta)
        {
            // calling the inputs from Player Controls Input System
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started; 

            // if inputted then roll 
            // if hold the button long else-- player will sprint instead than rolling. 
            if(b_Input)
            {
                RollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (RollInputTimer > 0 && RollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                RollInputTimer = 0; 
            }
        }
    }
}
