using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuTogglesController : MonoSinglethon<MenuTogglesController>
{
    [SerializeField] Toggle _menuToggle;
    private void Start()
    { 
        _menuToggle.onValueChanged.AddListener(SetInputState);
    }
    public void SetInputState(bool state)
    {
        CustomInputStateStatic.InputIsActivated = !state;
    }
     
}
