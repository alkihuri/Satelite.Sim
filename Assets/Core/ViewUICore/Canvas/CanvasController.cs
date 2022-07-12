using Entities.Orbit;
using UnityEngine;
using UnityEngine.UI;

namespace View.Canvas
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private OrbitManager _oManager;

        [SerializeField] private UniversalUIElementEffect[] _allEffects;
        
        [SerializeField] private UniversalUIElementEffect _mapPanel;
        [SerializeField] private UniversalUIElementEffect _satelitePanel;
        [SerializeField] private UniversalUIElementEffect _mainMenuPanel;
        
        [SerializeField] private Toggle _mainMenuToggle;
        [SerializeField] private Slider _slider;
        [SerializeField] private AnimationCurve _exponentionalCurve;
        
        private void Start()
        {
            _mainMenuToggle.onValueChanged.AddListener(MainMenuNotify);
            _slider.onValueChanged.AddListener(SetValueOfSatellites);
            
            UpdateValueOfSatellites();
        }
        private void MainMenuNotify(bool state)
        {
            _mainMenuPanel.SetActive(state);
        }

        public void ClearScene()
        {
            foreach (var effect in _allEffects)
            {
                effect.SetActive(false);
            }
        }

        public void SwitchMap()
        {
            _mapPanel.SetActive(!_mapPanel.gameObject.activeSelf);
        }

        public void ShowMap()
        {
            _mapPanel.SetActive(true);
            
            HideSatelliteInfoPanel();
        }
        
        public void HideMap()
        {
            _mapPanel.SetActive(false);
            
            ShowSatelliteInfoPanel();
        }
        
        public void ShowSatelliteInfoPanel()
        {
            _satelitePanel.SetActive(true);
        }
        
        public void HideSatelliteInfoPanel()
        {
            _satelitePanel.SetActive(false);
        }
        
        public void ShowSatelliteSlider()
        {
            _slider.gameObject.SetActive(true);
            UpdateValueOfSatellites();
        }
        
        public void HideSatelliteSlider()
        {
            _slider.gameObject.SetActive(false);
        }
        
        private void SetValueOfSatellites(float value)
        {
            _oManager.ShowAmountOfOrbits(_exponentionalCurve.Evaluate(value));
        }

        public void UpdateValueOfSatellites()
        {
            _oManager.ShowAmountOfOrbits(_exponentionalCurve.Evaluate(_slider.value));
        }
    }
}