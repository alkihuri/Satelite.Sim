using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[ExecuteAlways]
public class StylizedSliderVisual : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private RectTransform _handle;
    [SerializeField] private Camera _uiCamera;
    
    private Slider _slider;
    private static readonly int HandlePosition = Shader.PropertyToID("_HandlePosition");

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        UpdateVisuals();
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnValueChanged);
    }
    
    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        UpdateVisuals();
    }

    [ContextMenu("Update Visual")]
    private void UpdateVisuals()
    {
        float positionX = _uiCamera.WorldToScreenPoint(_handle.position).x / Screen.width;
        _background.material.SetFloat(HandlePosition, positionX);
    }
}
