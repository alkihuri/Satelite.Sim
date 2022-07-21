using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Settings
{
    public class SettingsLoader : MonoSinglethon<SettingsLoader>
    {

        private ApplicationSettings _settings;
        private string _jsonText;

        public ApplicationSettings Settings { get => _settings; set => _settings = value; }

        // Start is called before the first frame update
        void Awake()
        {
            ParseFile("example.json");
            ApplySettings();
            PlayerPrefsSync();
        }

        private void PlayerPrefsSync()
        {
            Sync();
            Logging();
        }

        private void Sync()
        {
            PlayerPrefs.SetFloat(SeetingsKeys.MouseWheelSpeed, _settings.MouseWheelSpeed);
            PlayerPrefs.SetFloat(SeetingsKeys.CameraRotatingSpeed, _settings.CameraRotatingSpeed);
            PlayerPrefs.SetFloat(SeetingsKeys.SwiperSpeedHorizontal, _settings.SwipeSpeedHorizontal);
            PlayerPrefs.SetFloat(SeetingsKeys.SwiperSpeedVertical, _settings.SwipeSpeedVertical);
        }

        private static void Logging()
        {
            Debug.Log("<color=green> Settings loaded:</color>");
            Debug.Log("<color=green> Mouse wheel speed :" + PlayerPrefs.GetFloat(SeetingsKeys.MouseWheelSpeed) + "</color>");
            Debug.Log("<color=green> Camera rotate speed :" + PlayerPrefs.GetFloat(SeetingsKeys.CameraRotatingSpeed) + "</color>");
            Debug.Log("<color=green> Swipe speed Horizontal :" + PlayerPrefs.GetFloat(SeetingsKeys.SwiperSpeedHorizontal) + "</color>");
            Debug.Log("<color=green> Swipe speed Vertical :" + PlayerPrefs.GetFloat(SeetingsKeys.SwiperSpeedVertical) + "</color>");
        }

        private void ApplySettings()
        {
            _settings = ApplicationSettings.CreateSettings(_jsonText);
        }

        private void ParseFile(string file)
        {
            _jsonText = GetText(file);
        }

        private static string GetText(string file)
        {
            return File.ReadAllText(Application.streamingAssetsPath + "/Settings/" + file);
        }
    }

}