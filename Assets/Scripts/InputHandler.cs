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
        public float mouseX;
        public float mouseY;

        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker; 
        private PlayerManager playerManager;
        PlayerInventory playerInventory;
        private CameraHandler cameraHandler;


        private Vector2 movementinput;
        private Vector2 cameraInput;


        public bool b_Input;
        public bool rb_Input;
        public bool rt_Input;

        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public float RollInputTimer;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();  
            playerManager = GetComponent<PlayerManager>();
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
            HandleAttackInput(delta);
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


        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => rb_Input = true; 
            inputActions.PlayerActions.RT.performed += i => rt_Input = true; 

            // RB inputs deal with right hand. 
            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false; 
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                    if (playerManager.canDoCombo)
                        return;
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
                
            }
            if (rt_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                    if (playerManager.canDoCombo)
                        return;
                    playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);

                }
                
            }
        }
    }
}
