using Assets.SimpleLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class LocalizationSettings : MonoBehaviour
    {
        private void Awake()
        {
            LocalizationManager.Read();
            LocalizationManager.Language = "English"; 
        }
         
        public void SetLang(string lang)
        {
            LocalizationManager.Read();
            LocalizationManager.Language = lang;
        }
    }
}