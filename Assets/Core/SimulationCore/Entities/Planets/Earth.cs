using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intefaces;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Earth : MonoBehaviour, ISelectable, IPlanet
{
    [SerializeField] private Transform _visual;
    
    public bool IsSelected { get; private set; }

    private void Awake()
    {
        if (_visual == null) _visual = transform;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        if (IsSelected == false)
        {
            if (SelectionManager.Instance.TrySelect(this) == false) return;
            
            CameraSettings.CameraController.Instance.SwitchCamera(gameObject);
            IsSelected = true;
        }
    }
    
    public void DeSelect()
    {
        if (IsSelected)
        {
            CameraSettings.CameraController.Instance.SwitchCamera(gameObject);
            IsSelected = false;
        }
    }
    
    private void Update()
    {
        Spin();
    }

    public void Spin()
    {
        _visual.Rotate(0, Constants.SimulatedEarthAngularSpeed * Time.deltaTime, 0);   
    }

}
