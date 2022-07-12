using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CameraSettings
{
    public class CameraSceneSettings : MonoBehaviour
    {
        [SerializeField] GameObject _earth;
        // Start is called before the first frame update
        void Start()
        {
                // Cursor.lockState = CursorLockMode.Locked;
            CameraController.Instance.CamerasInnit(_earth);
        } 
    }
}
