using System;
using System.Collections;
using Entities.Orbit;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Entities.Orbit.SateliteInfoParser;

namespace DataControllers
{

    public class CoordinateHandler : MonoBehaviour
    {
        [SerializeField] private int _predictionDeltaTime;
        [SerializeField] private Vector2Int _predictionsRange;
        [SerializeField] private float _gapDistanceThreshold = 0.5f;
        
        private OrbitController _orbitController;
        private Satellite _satellite;
        private Coroutine _predictionRoutine;

        private void OnValidate()
        {
            if (_predictionsRange.y < _predictionsRange.x)
                _predictionsRange.y = _predictionsRange.x;
        }

        private void Awake()
        {
            _orbitController = GetComponentInParent<OrbitController>();
            _satellite = GetComponent<Satellite>();
        }

        private void OnEnable()
        {
            _satellite.OnSelect.AddListener(StartPrediction);
            _satellite.OnDeselect.AddListener(StopPrediction);
        }
        
        private void OnDisable()
        {
            _satellite.OnSelect.RemoveListener(StartPrediction);
            _satellite.OnDeselect.RemoveListener(StopPrediction);
        }

        private void StartPrediction()
        {
            if (_predictionRoutine == null)
                _predictionRoutine = StartCoroutine(UpdatePredictionRoutine());
        }
        
        private void StopPrediction()
        {
            if (_predictionRoutine != null)
            {
                StopCoroutine(_predictionRoutine);
                _predictionRoutine = null;
            }
        }

        private IEnumerator UpdatePredictionRoutine()
        {
            while (true)
            {
                Vector2 currentPosition2D = ProjectPosition2D();
                List<Vector3> path3D = PredictPath3D();
                List<Vector2> path2D = Convert3DListOfPointsTo2DList(path3D);
                List<int> gapIndexes = new List<int>();

                for (int i = 0; i < path2D.Count - 1; i++)
                {
                    if (Vector2.Distance(path2D[i], path2D[i + 1]) > _gapDistanceThreshold)
                        gapIndexes.Add(i);
                }
                
                CoordinateHandlerForSatellite.Instance.UPDATE_MARKERS(path2D, gapIndexes, currentPosition2D);
                
                yield return null;
            }
        }

        private Vector2 ProjectPosition2D()
        {
            Vector3 projectedPosition = PredictProjection3D(0);
            return Convert3DPointTo2DPointOnMap(projectedPosition);
        }

        private List<Vector3> PredictPath3D()
        {
            List<Vector3> predictedPoints3D = new List<Vector3>();
            for (int i = _predictionsRange.x; i < _predictionsRange.y; i++)
            {
                predictedPoints3D.Add(PredictProjection3D(_predictionDeltaTime * i));
            }
            return predictedPoints3D;
        }

        private Vector3 PredictProjection3D(int time)
        {
            Vector3 currentProjection = _orbitController.transform.forward * (Constants.ScaledEarthRadius + 0.1f);
            
            Quaternion centerDeltaRotation =
                Quaternion.AngleAxis(-Constants.EarthAngularSpeed * time, _orbitController.OrbitedBody.up);
            Quaternion orbitDeltaRotation =
                Quaternion.AngleAxis(_orbitController.ANGULAR_SPEED * time, _orbitController.transform.up);

            return centerDeltaRotation * (orbitDeltaRotation * currentProjection);
        }

        private List<Vector2> Convert3DListOfPointsTo2DList(List<Vector3> points)
        {
            List<Vector2> points2D = new List<Vector2>();
            foreach (Vector3 point in points)
            {
                var point2D = Convert3DPointTo2DPointOnMap(point);
                points2D.Add(point2D);
            }

            return points2D;
        }

        private Vector2 Convert3DPointTo2DPointOnMap(Vector3 position)
        {
            Vector2 pixelUV = new Vector2(0, 0);
            var globalZero = _orbitController.transform.position;
            var globalStartPosition = position;
            if (Physics.Raycast(globalStartPosition, globalZero - globalStartPosition, out RaycastHit hit))
            {
                var hitPoint = hit.point;
                Debug.DrawLine(position, hitPoint, Color.green, .5f);
                pixelUV = hit.textureCoord; 
            }
            return pixelUV;
        }
    }

}