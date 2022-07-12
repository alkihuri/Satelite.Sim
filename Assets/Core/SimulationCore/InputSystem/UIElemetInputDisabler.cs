using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElemetInputDisabler : MonoBehaviour
{

    private void OnEnable()
    {
        CustomInputStateStatic.InputIsActivated = false; 
    }

    private void OnDisable()
    {
        CustomInputStateStatic.InputIsActivated = true;
    }
}
