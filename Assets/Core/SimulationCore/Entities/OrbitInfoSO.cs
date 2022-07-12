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
        [SerializeField] private float _year;
        [SerializeField] private Satellite _satellitePrefab;

        public float Incline => _incline;
        public float InitialPhase => _initialPhase;
        public float InitialRotation => _initialRotation;
        public float Radius => _radius;
        public float Year => _year;
        public Satellite SatellitePrefab => _satellitePrefab;
    }
}