using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineShpereDisplacer : MonoBehaviour
{
    
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineWidth;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private int _pointCount;

    [SerializeField] private bool _isOnTheEdge;
    [SerializeField] private float _displacementSphereRadius;

    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private void Update()
    {
        if (!Application.isPlaying) DrawLine();
    }

    private void DrawLine()
    {
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.colorGradient = _gradient;
        // _lineRenderer.startColor = _color;
        // _lineRenderer.endColor = _color;
        // for (int i = 0; i < _lineRenderer.colorGradient.alphaKeys.Length; i++)
        // {
        //     _lineRenderer.colorGradient.alphaKeys[i].alpha = _color.a;
        // }

        _lineRenderer.positionCount = _pointCount;
        
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            Vector3 pointALocal = _lineRenderer.transform.InverseTransformPoint(_pointA.position);
            Vector3 pointBLocal = _lineRenderer.transform.InverseTransformPoint(_pointB.position);
            
            Vector3 lineSample = (pointBLocal - pointALocal) / (_pointCount-1) * i + pointALocal;
            Vector3 direction = _isOnTheEdge ? lineSample - Vector3.forward * _displacementSphereRadius : lineSample;
            Vector3 delta = direction - direction.normalized * _displacementSphereRadius;
            Vector3 finalPosition = lineSample - delta;

            _lineRenderer.SetPosition(i, finalPosition);
        }
    }
}
