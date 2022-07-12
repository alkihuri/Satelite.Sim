using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public class UniversalInput : MonoSinglethon<UniversalInput>
    {

        float _vertical;
        float _horizontal;
        float _cameraZoom;
        public float VERTICAL
        {
            get
            {
                return Mathf.Clamp(_vertical, -1, 1);
            }
            set
            {
                _vertical = value;
            }
        }
        public float HORIZONTAL
        {
            get
            {
                return Mathf.Clamp(_horizontal, -1, 1);
            }
            set
            { 
                _horizontal = value;
            }
        }
        public float CAMERA_ZOOM
        {
            get
            {
                return Mathf.Clamp(_cameraZoom, -1, 1);
            }
            set
            {
                _cameraZoom = value;
            }
        }
    }
}

