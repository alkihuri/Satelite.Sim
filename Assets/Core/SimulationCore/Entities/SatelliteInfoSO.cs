using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "New Satellite", menuName = "Custom/Satellite", order = 0)]
    public class SatelliteInfoSO : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _country;
        [SerializeField] private string _mass;
        [SerializeField] private string _dedication;
        [SerializeField] private string _launchData;
        [SerializeField] private string _mse;
        [SerializeField] private string _lifetime;
        [SerializeField, TextArea] private string _constructionDetailedInfo;

        public string Name => _name;
        public string Country => _country;
        public string Mass => _mass;
        public string Dedication => _dedication;
        public string LaunchData => _launchData;
        public string MSE => _mse;
        public string Lifetime => _lifetime;
        public string ConstructionDetailedInfo => _constructionDetailedInfo;
    }
}