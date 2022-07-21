using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using Entities;
using TMPro;
using UnityEngine;

public class SatelliteNamePlate : MonoBehaviour
{
    [SerializeField] private Satellite _satellite;
    
    private TMP_Text _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
        LocalizationManager.LocalizationChanged += UpdateText;
    }

    private void Start()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _tmpText.text = LocalizationManager.Localize(_satellite.Info.Name);
    }

    private void OnDestroy()
    {
        LocalizationManager.LocalizationChanged -= UpdateText;
    }
}
