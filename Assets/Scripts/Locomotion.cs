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

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private GameObject normalCamera;

        [Header("Stats")]
        [SerializeField] float movementSpeed = 5;
        [SerializeField] float rotationSpeed = 10;




        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;

        }

        public void Update()
        {
            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            moveDirection.Normalize();

            float speed = movementSpeed;
            moveDirection *= speed;


            Vector3 projectVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectVelocity;

        }


        #region Movement
        private Vector3 normalVector;
        private Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;


            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.forward * inputHandler.horizontal;


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
        #endregion

}
}

