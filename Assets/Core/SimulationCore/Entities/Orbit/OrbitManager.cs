using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.Canvas;
using Entities.Orbit.SateliteInfoParser;
using ModestTree;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities.Orbit
{
    [Serializable]
    public class OrbitDistribution
    {
        [Range(0, 1)] public float Random;
        [Range(0, 1)] public float Low;
        [Range(0, 1)] public float LowEquator;
        [Range(0, 1)] public float Medium;
        [Range(0, 1)] public float Geostationary;
        [Range(0, 1)] public float MoonRandom;
    }

    public class OrbitManager : EntityManager
    {
        [SerializeField] private bool _showFirstOrbit = true;
        
        [SerializeField] private int _numOfOrbits = 5000;
        [SerializeField] private OrbitController _orbitPrefab;
        [SerializeField] private CanvasController _canvas;  // no time for MVC
        [SerializeField] private Transform _orbitedBody;
        [SerializeField] private OrbitDistribution _orbitDistribution;
        [SerializeField] private OrbitInfoSO[] _realOrbitsInfo;
        [SerializeField] private OrbitInfoSO _firstOrbitInfo;
        
        private List<Transform> _orbits = new List<Transform>();
        private List<GameObject> _poolOfSatellites = new List<GameObject>();
        private List<Satellite> _realSatellites = new List<Satellite>();
        private List<OrbitController> _poolOfRandomOrbits = new List<OrbitController>();
        private List<OrbitController> _poolOfRealOrbits = new List<OrbitController>();
        
        private NativeArray<float> _speedsArray;
        private OrbitUpdateJob _orbitUpdateJob;
        private TransformAccessArray _orbitTransformAccesses;
        private List<OrbitType> _rolledOrbits = new List<OrbitType>();

        // private const int NUM_OF_REAL_SATELITES = 10;

        void Awake()
        {
            InitFirstOrbit();
            InnitPoolOfRandomOrbits();
            InnitPoolOfRealOrbits();
            
            InnitPoolOfSatellites();

            InitOrbitJobs();
        }

        private void Update()
        {
            UpdateOrbits();
        }

        private void OnDestroy()
        {
            _speedsArray.Dispose();
            _orbitTransformAccesses.Dispose();
        }
        
        private void InnitPoolOfSatellites()
        {
            SetupOrbitsTransforms();
            RandomSatellitesInnit();
            RealSatelliteInnit();
            SatelliteIconsInit();
        }
        
        private void SetupOrbitsTransforms()
        {
            _orbits = _poolOfRandomOrbits.Concat(_poolOfRealOrbits).Select(o => o.transform).ToList();
        }

        private void RealSatelliteInnit()
        {
            for (int i = 0; i < _realOrbitsInfo.Length; i++)
            {
                Satellite newSatellite = Instantiate(
                    _realOrbitsInfo[i].SatellitePrefab, 
                    _poolOfRealOrbits[i].transform.position + _poolOfRealOrbits[i].transform.forward * _poolOfRealOrbits[i].ORBIT_VISUAL_RADIUS * Constants.Scale,
                    _poolOfRealOrbits[i].transform.rotation,
                    _poolOfRealOrbits[i].transform);
                
                EventsSettings(newSatellite);

                _poolOfSatellites.Add(newSatellite.gameObject);
                _realSatellites.Add(newSatellite);
            }
        } 
        private void RandomSatellitesInnit()
        {
            foreach (var orbit in _poolOfRandomOrbits)
            {
                GameObject newSatellite = new GameObject("Satellite");
                newSatellite.transform.parent = orbit.transform;

                newSatellite.transform.localPosition = Vector3.forward * orbit.ORBIT_VISUAL_RADIUS * Constants.Scale;
                _poolOfSatellites.Add(newSatellite);
            }
        }
        
        private void SatelliteIconsInit()
        {
            GetComponent<IconDrawer>().Transforms = _poolOfSatellites.Select(s => s.transform).ToArray();
        }

        private void EventsSettings(Satellite newSatellite)
        {
            newSatellite.OnSelect.AddListener(_canvas.ShowSatelliteInfoPanel);
            newSatellite.OnSelect.AddListener(_canvas.HideSatelliteSlider);
            newSatellite.OnDeselect.AddListener(_canvas.HideSatelliteInfoPanel);
            newSatellite.OnDeselect.AddListener(_canvas.ShowSatelliteSlider);

            newSatellite.OnSelect.AddListener(HideAllExcept);
            newSatellite.OnDeselect.AddListener(ShowAllSatellite);
        }


        private void InitOrbitJobs()
        {
            List<OrbitController> orbits = _poolOfRandomOrbits.Concat(_poolOfRealOrbits).ToList();
            _speedsArray = new NativeArray<float>(orbits.Count, Allocator.Persistent);
            for (int i = 0; i < orbits.Count; i++)
            {
                _speedsArray[i] = orbits[i].ANGULAR_SPEED;
            }
            
            _orbitUpdateJob = new OrbitUpdateJob
            {
                AngularSpeeds = _speedsArray,
                DeltaTime = Time.deltaTime
            };
            _orbitTransformAccesses = new TransformAccessArray(_orbits.ToArray(), 5);
        }
        
        private void UpdateOrbits()
        {
            JobHandle jobHandle = _orbitUpdateJob.Schedule(_orbitTransformAccesses);
            jobHandle.Complete();
        }

        private void InitFirstOrbit()
        {
            if (_firstOrbitInfo == null) return;

            OrbitController newOrbit = Instantiate(_orbitPrefab, _orbitedBody.position, _orbitedBody.rotation, transform);
            newOrbit.name = _firstOrbitInfo.name + "_orbit";
            newOrbit.SetupOrbit(_firstOrbitInfo, _orbitedBody);
            _poolOfRandomOrbits.Add(newOrbit);
        }

        private void InnitPoolOfRandomOrbits()
        {
            for (int i = 0; i < _numOfOrbits; i++)
            {
                OrbitController newOrbit = Instantiate(_orbitPrefab, _orbitedBody.position, _orbitedBody.rotation, transform);
                newOrbit.name = i.ToString() + "_orbit";
                
                GenerateRandomOrbit(out float radius, out Vector3 rotation, out bool drawOrbit);
                
                newOrbit.SetupOrbit(radius, rotation, _orbitedBody, drawOrbit);
                
                _poolOfRandomOrbits.Add(newOrbit);
            }
        }
        
        private void InnitPoolOfRealOrbits()
        {
            foreach (OrbitInfoSO orbitInfo in _realOrbitsInfo)
            {
                OrbitController newOrbit = Instantiate(_orbitPrefab, _orbitedBody.position, _orbitedBody.rotation, transform);
                newOrbit.name = orbitInfo.name + "_orbit";
                newOrbit.SetupOrbit(orbitInfo, _orbitedBody);
                _poolOfRealOrbits.Add(newOrbit);
            }
        }
        
        private void GenerateRandomOrbit(out float radius, out Vector3 rotation, out bool drawOrbit)
        {
            float[] orbitWeights = {
                Random.Range(0f, 1f) * _orbitDistribution.Random,
                Random.Range(0f, 1f) * _orbitDistribution.Low,
                Random.Range(0f, 1f) * _orbitDistribution.LowEquator,
                Random.Range(0f, 1f) * _orbitDistribution.Medium,
                Random.Range(0f, 1f) * _orbitDistribution.Geostationary,
                Random.Range(0f, 1f) * _orbitDistribution.MoonRandom
            };
            var randomOrbit = (OrbitType)orbitWeights.IndexOf(orbitWeights.Max());
                
            drawOrbit = false;
            if (_rolledOrbits.Contains(randomOrbit) == false)
            {
                _rolledOrbits.Add(randomOrbit);
                drawOrbit = true;
            }
            
            float lowerBound;
            float upperBound;
            
            switch (randomOrbit)
            {
                case OrbitType.Low:
                    lowerBound = (Constants.EarthRadius + Constants.LowOrbitAltitude.x);
                    upperBound = (Constants.EarthRadius + Constants.LowOrbitAltitude.y);
                    radius = Random.Range(lowerBound, upperBound);
                    rotation =
                        new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    break;
                case OrbitType.LowEquator:
                    lowerBound = (Constants.EarthRadius + Constants.LowOrbitAltitude.x);
                    upperBound = (Constants.EarthRadius + Constants.LowOrbitAltitude.y);
                    radius = Random.Range(lowerBound, upperBound);
                    rotation =
                        new Vector3(Random.Range(-50f, 50f), Random.Range(-180f, 180f), Random.Range(-50f, 50f));
                    break;
                case OrbitType.Medium:
                    lowerBound = (Constants.EarthRadius + Constants.MediumOrbitAltitude.x);
                    upperBound = (Constants.EarthRadius + Constants.MediumOrbitAltitude.y);
                    radius = Random.Range(lowerBound, upperBound);
                    rotation =
                        new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    break;
                case OrbitType.Geostationary:
                    lowerBound = (Constants.EarthRadius + Constants.GeostationaryOrbitAltitude.x);
                    upperBound = (Constants.EarthRadius + Constants.GeostationaryOrbitAltitude.y);
                    radius = Random.Range(lowerBound, upperBound);
                    rotation =
                        new Vector3(Random.Range(-5f, 5f), Random.Range(-180f, 180f), Random.Range(-5f, 5f));
                    break;
                case OrbitType.Random:
                    lowerBound = (Constants.EarthRadius + Constants.LowOrbitAltitude.x);
                    upperBound = (Constants.EarthRadius + Constants.GeostationaryOrbitAltitude.y);
                    radius = Random.Range(lowerBound, upperBound);
                    rotation =
                        new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    break;
                case OrbitType.MoonRandom:
                    lowerBound = (Constants.MoonRadius + Constants.MoonOrbitAltitude.x);
                    upperBound = (Constants.MoonRadius + Constants.MoonOrbitAltitude.y);
                    radius = Random.Range(lowerBound, upperBound);
                    rotation =
                        new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    break;
                default:
                    radius = 0;
                    rotation = Vector3.zero;
                    break;
            }
        }

        public override void ShowAmountOfEntities(float n)
        {
            float amount = n * _orbits.Count;
            if (_showFirstOrbit) amount += 1; // first satellite is always shown
            
            for (int x = 0; x < _orbits.Count; x++)
            {
                var state = amount > x;
                if (_orbits[x].gameObject.activeSelf != state)
                    _orbits[x].gameObject.SetActive(state);
            }
        }

        private void HideAllExcept()
        {
            StartCoroutine(DelayHide());
        }
        
        private void ShowAllSatellite()
        {
            foreach (var orbit in _orbits) // тут было бы либо по спутникам либо по орбитам
            {
                var orbitLine = orbit.GetComponent<OrbitLine>();
                orbitLine.SetTransparency(1);
                // orbitLine.SetWidth(0.04f);
            }
        }

        private IEnumerator DelayHide()
        {
            yield return new WaitForSeconds(1);
            foreach (OrbitController orbitController in _poolOfRandomOrbits.Concat(_poolOfRealOrbits))
            {
                var orbit = orbitController.GetComponent<OrbitLine>();
                orbit.SetTransparency(0.025f);

                // if (satelliteObject.TryGetComponent(out Satellite satellite))
                // {
                //     if (satellite.IsSelected)
                //         orbit.SetWidth(0.005f);
                // }
            }
        }
    }
}