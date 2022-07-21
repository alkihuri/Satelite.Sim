using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDisplayManager : MonoBehaviour
{
    [SerializeField] private bool _enableMultiDisplay;
    
    void Awake()
    {
        if (!_enableMultiDisplay) return;
        
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
