using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Settings
{


    [System.Serializable]
    public class ApplicationSettings
    {
        public float MouseWheelSpeed;
        public float CameraRotatingSpeed;
        public float SwipeSpeedHorizontal;
        public float SwipeSpeedVertical;

        public static ApplicationSettings CreateSettings(string json)
        { 
            return JsonUtility.FromJson<ApplicationSettings>(json); 
        }
    }
}