using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Entities;
using Assets.SimpleLocalization;

public class SelectebleObjectInfo : MonoSinglethon<SelectebleObjectInfo>
{
    [SerializeField] TMP_Text[] _infoArray;

    protected override void Awake()
    {
        base.Awake();
        
        _infoArray.Initialize();
    }

    public void ApplyInfo(SatelliteInfoSO info)
    {
        if (info == null)
            return;

        try
        {
            _infoArray[0].text = LocalizationManager.Localize(info.Name);
            _infoArray[1].text = LocalizationManager.Localize(info.Country);
            _infoArray[2].text = LocalizationManager.Localize(info.Mass);
        }
        catch
        {
            _infoArray[0].text = info.Name;
            _infoArray[1].text = info.Country;
            _infoArray[2].text = info.Mass;
        }
    }
}
