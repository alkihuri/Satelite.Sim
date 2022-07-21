using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace Entities.Orbit
{
    public struct OrbitUpdateJob : IJobParallelForTransform
    {
        public NativeArray<float> AngularSpeeds;
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            var speed = AngularSpeeds[index];
            
            transform.rotation *=
                Quaternion.Euler(0, speed * DeltaTime * Constants.SimulationSpeedMultiplier, 0);

            AngularSpeeds[index] = speed;
        }
    }
    
    public class OrbitController : MonoBehaviour
    {
        private float _orbitRadius;
        private float _orbitVisualRadius;
        private float _angularSpeed;
        private Transform _orbitedBody;
        private OrbitLine _orbitLine;
        
        public float ORBIT_RADIUS => _orbitRadius;
        public float ORBIT_VISUAL_RADIUS => _orbitVisualRadius;
        public float ANGULAR_SPEED => _angularSpeed;
        public Transform OrbitedBody => _orbitedBody;

        private void Awake()
        {
            _orbitLine = GetComponent<OrbitLine>();
        }

        public void SetupOrbit(float radius, Vector3 rotation, Transform orbitedBody, bool drawOrbit)
        {
            _orbitRadius = radius;
            _orbitVisualRadius = radius;
            _orbitedBody = orbitedBody;
            _orbitLine.DRAW_ORBIT = drawOrbit;
            transform.localEulerAngles = rotation;
            SetupAngularSpeed();
        }

        public void SetupOrbit(OrbitInfoSO orbitInfo, Transform orbitedBody)
        {
            _orbitRadius = orbitInfo.Radius;
            _orbitVisualRadius = orbitInfo.Radius + orbitInfo.AdditionalRadius;
            _orbitedBody = orbitedBody;
            _orbitLine.DRAW_ORBIT = true;
            _orbitLine.SetColor(orbitInfo.OrbitColor);
            transform.localEulerAngles = Vector3.forward * orbitInfo.Incline;
            transform.Rotate(transform.up, orbitInfo.InitialPhase, Space.World);
            transform.Rotate(orbitedBody.up, orbitInfo.InitialRotation, Space.World);
            SetupAngularSpeed();
        }

        private void SetupAngularSpeed()
        {
            _angularSpeed =
                Mathf.Sqrt(Constants.GravitationalConstant * Constants.EarthMass / Mathf.Pow(_orbitRadius * 1000f, 3)) *
                Mathf.Rad2Deg;
        }
    }
}