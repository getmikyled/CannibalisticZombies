using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{
    ///-////////////////////////////////////////////////////////////
    ///
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] float sensitivityX = 200f;
        [SerializeField] float sensitivityY = 200f;

        [SerializeField] Transform playerOrientation;
        [SerializeField] Transform cameraPosition;

        private float xRotation;
        private float yRotation;

        ///-////////////////////////////////////////////////////////////
        ///
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        ///-////////////////////////////////////////////////////////////
        ///
        private void Update()
        {
            MoveCamera();
            RotateCamera();
        }

        ///-////////////////////////////////////////////////////////////
        ///
        private void MoveCamera()
        {
            transform.position = cameraPosition.position;
        }

        ///-////////////////////////////////////////////////////////////
        ///
        private void RotateCamera()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY * -1;

            yRotation += mouseX;
            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

}