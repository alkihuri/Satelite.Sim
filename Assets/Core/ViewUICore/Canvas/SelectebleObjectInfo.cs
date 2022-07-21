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
    private SatelliteInfoSO _info;
    [SerializeField] GameObject _mapBtn;
    protected override void Awake()
    {
        base.Awake();

        _infoArray.Initialize();
        LocalizationManager.LocalizationChanged += UpdateInfo;
    }

    public void ApplyInfo(SatelliteInfoSO info)
    {
        if (info == null)
            return;
        _info = info;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (_info == null)
            return;

        _mapBtn.SetActive(!_info.IsMoonSatelite);

        try
        {
            _infoArray[0].text = LocalizationManager.Localize(_info.Name);
            _infoArray[1].text = LocalizationManager.Localize(_info.Country);
            _infoArray[2].text = LocalizationManager.Localize(_info.Mass);
            _infoArray[3].text = LocalizationManager.Localize(_info.Dedication);
            _infoArray[4].text = LocalizationManager.Localize(_info.ConstructionDetailedInfo);
        }
        catch
        {
            _infoArray[0].text = _info.Name;
            _infoArray[1].text = _info.Country;
            _infoArray[2].text = _info.Mass;
        }
    }
}
