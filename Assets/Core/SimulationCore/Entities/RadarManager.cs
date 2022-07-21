using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class RadarManager : EntityManager
    {
        [SerializeField] private bool _generateOnStart;
        [SerializeField] private float _sphereRadius;
        [SerializeField] private int _numOfRadars = 100;
        [SerializeField] private List<Transform> _radarList = new List<Transform>();
        
        private void Start()
        {
            if (_generateOnStart) InitPoolOfRadars();
        }

        private void InitPoolOfRadars()
        {
            for (int i = 0; i < _numOfRadars; i++)
            {
                Vector3 randomPosition = GetRandomPositionOnSphere();
                
                GameObject radar = new GameObject
                {
                    name = "Radar",
                    transform =
                    {
                        position = randomPosition,
                        rotation = Quaternion.LookRotation(-randomPosition) * Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.forward),
                        parent = transform
                    }
                };
                _radarList.Add(radar.transform);
            }

            GetComponent<IconDrawer>().Transforms = _radarList.ToArray();
        }

        private Vector3 GetRandomPositionOnSphere()
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);

            return new Vector3(x, y, z).normalized * _sphereRadius;
        }

        public override void ShowAmountOfEntities(float n)
        {
            float amount = n * _numOfRadars;
            
            for (int x = 0; x < _radarList.Count; x++)
            {
                var state = amount > x;
                if (_radarList[x].gameObject.activeSelf != state)
                    _radarList[x].gameObject.SetActive(state);
            }
        }
    }
}
