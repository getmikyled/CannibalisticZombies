using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{
    ///-/////////////////////////////////////////////////////////////////////
    ///
    /// Camera Code
    /// 
    public class PCamera : MonoBehaviour
    {
        public float sensX;
        public float sensY;
        [SerializeField] Transform cameraPos;
        public Transform orientation;

        float xRotation;
        float yRotation;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            transform.position = cameraPos.position;
        }
    }
}
