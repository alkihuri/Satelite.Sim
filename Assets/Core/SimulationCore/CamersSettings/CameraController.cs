using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace CameraSettings
{
    public class CameraController : MonoSinglethon<CameraController>
    {
        CinemachineVirtualCameraBase _prevCamera;
        CinemachineVirtualCameraBase _currentCamera;

        const int MIN_PRIORITY = 0;
        const int MAX_PRIORITY = 1;

        public void CamerasInnit(GameObject gameObject)
        {
            _prevCamera = gameObject.GetComponentInChildren<CinemachineVirtualCameraBase>();
            _currentCamera = gameObject.GetComponentInChildren<CinemachineVirtualCameraBase>();
        }
        public void SwitchCamera(GameObject gameObject)
        {
            if (!GetComponentInChildren<CinemachineVirtualCameraBase>())
                return;

            _prevCamera.m_Priority = MIN_PRIORITY;
            _currentCamera = gameObject.GetComponentInChildren<CinemachineVirtualCameraBase>();
            _currentCamera.m_Priority = MAX_PRIORITY;
            _prevCamera = _currentCamera;
        }

        internal void SwitchCamera(object earth)
        {
            throw new NotImplementedException();
        }

        public void SwitchCamera(GameObject gameObject, Entities.Satellite context)
        {
            context.OnDeselect.Invoke();
            _prevCamera.m_Priority = MIN_PRIORITY;
            _currentCamera = gameObject.GetComponentInChildren<CinemachineVirtualCameraBase>();
            _currentCamera.m_Priority = MAX_PRIORITY;
            _prevCamera = _currentCamera;
        }

    }
}


