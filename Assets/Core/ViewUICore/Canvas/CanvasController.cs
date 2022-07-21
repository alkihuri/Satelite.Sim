using Entities.Orbit;
using UnityEngine;
using UnityEngine.UI;

namespace View.Canvas
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private EntityManager[] _entityManagers;

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
            _slider.onValueChanged.AddListener(SetAmountOfEntities);
            
            UpdateAmountOfEntities();
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
            UpdateAmountOfEntities();
        }
        
        public void HideSatelliteSlider()
        {
            _slider.gameObject.SetActive(false);
        }
        
        public void SetAmountOfEntities(float value)
        {
            foreach (var entityManager in _entityManagers)
            {
                entityManager.ShowAmountOfEntities(value >= 0 ? _exponentionalCurve.Evaluate(value) : value);
            }
        }

        public void UpdateAmountOfEntities()
        {
            foreach (var entityManager in _entityManagers)
            {
                entityManager.ShowAmountOfEntities(_exponentionalCurve.Evaluate(_slider.value));
            }
        }
    }
}