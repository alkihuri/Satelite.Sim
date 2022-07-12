using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace InputSystem
{
    public class InputBridge : MonoBehaviour
    {


        private void FixedUpdate()
        {
            UniversalInput.Instance.VERTICAL += Input.GetAxis("Vertical");
            UniversalInput.Instance.HORIZONTAL += Input.GetAxis("Horizontal");
            UniversalInput.Instance.CAMERA_ZOOM += Input.GetAxis("Mouse ScrollWheel");
        }

    }
}


