using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // variables for getting the inputs 

    public float horizontal;
    public float vertical;
    public float moveAmount;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;

   private PlayerControls inputActions;

   private Vector2 movementinput;
   private Vector2 cameraInput;



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

    public void TickInput(float delta)
    {
        MoveInput(delta);
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
}
