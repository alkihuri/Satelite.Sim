using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.SimpleLocalization;

namespace Localization.Custom
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProLocalized : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        [SerializeField] private string _key;
        // Start is called before the first frame update
        void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        public void Start()
        {
            LocalizeTMPro();
            LocalizationManager.LocalizationChanged += LocalizeTMPro;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= LocalizeTMPro;
        }
        private void LocalizeTMPro()
        {
            _text.text = LocalizationManager.Localize(_key);
        }
    }
}