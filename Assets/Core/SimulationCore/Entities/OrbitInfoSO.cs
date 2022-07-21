using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "New Orbit", menuName = "Custom/Orbit", order = 0)]
    public class OrbitInfoSO : ScriptableObject
    {
        [SerializeField] private float _incline;
        [SerializeField] private float _initialPhase;
        [SerializeField] private float _initialRotation;
        [SerializeField] private float _radius;
        [SerializeField] private float _additionalRadius;
        [SerializeField] private float _year;
        [SerializeField] private Satellite _satellitePrefab;
        [SerializeField] private Color _orbitColor;

        public float Incline => _incline;
        public float InitialPhase => _initialPhase;
        public float InitialRotation => _initialRotation;
        public float Radius => _radius;
        public float AdditionalRadius => _additionalRadius;
        public float Year => _year;
        public Satellite SatellitePrefab => _satellitePrefab;
        public Color OrbitColor => _orbitColor;
    }
}