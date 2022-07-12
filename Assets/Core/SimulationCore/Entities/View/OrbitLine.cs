using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Entities.Orbit
{
    public class OrbitLine : MonoBehaviour
    {
        [SerializeField] private float _chanceOfRender;
        [SerializeField] private Color _color1;
        [SerializeField] private Color _color2;
        
        private const int TRAJECTORY_RESOLUTION = 100;
        private LineRenderer _lineRenderer;
        private float _radius;
        private List<Vector3> _currentOrbitPoints = new List<Vector3>();
        private bool _drawOrbit;

        public bool DRAW_ORBIT
        {
            set
            {
                _drawOrbit = value;
                
                if (value)
                {
                    ShowOrbitLine();
                }
                else
                {
                    HideOrbitLine();
                }
            }
        }
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void Start()
        {
            if (Random.Range(0f, 1f) < _chanceOfRender) _drawOrbit = true;
            if (!_drawOrbit) return;
            
            _radius = GetComponent<OrbitController>().ORBIT_RADIUS;
            LineRendererSettings();
            DrawOrbit();
        }

        private void HideOrbitLine()
        {
            if (_lineRenderer != null)
                _lineRenderer.enabled = false;
        }

        private void ShowOrbitLine()
        {
            if (_lineRenderer != null)
                _lineRenderer.enabled = true;
        }


        private void LineRendererSettings()
        {
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = TRAJECTORY_RESOLUTION;
            
            Color randomColor = Random.Range(0, 2) > 0 ? _color1 : _color2;
            _lineRenderer.startColor = randomColor;
            _lineRenderer.endColor = randomColor;
        }

        private void DrawOrbit()
        {
            _lineRenderer.positionCount = TRAJECTORY_RESOLUTION;
            for (int x = 0; x < TRAJECTORY_RESOLUTION; x++)
            {
                Vector3 nextVector = new Vector3(
                    Mathf.Sin(x * Mathf.PI * 2 / TRAJECTORY_RESOLUTION) * _radius * Constants.Scale,
                    0,
                    Mathf.Cos(x * Mathf.PI * 2 / TRAJECTORY_RESOLUTION) * _radius * Constants.Scale
                    );
                _currentOrbitPoints.Add(nextVector);
                _lineRenderer.SetPosition(x, nextVector);
            }
        }

        public void SetTransparency(float alpha)
        {
            if (_drawOrbit)
            {
                Color oldColor = _lineRenderer.material.color;
                _lineRenderer.material.DOColor(new Color(oldColor.r, oldColor.g, oldColor.b, alpha), 1);
            }
        }

        public void SetWidth(float width)
        {
            if (_drawOrbit)
                _lineRenderer.startWidth = width;
        }
    }
}