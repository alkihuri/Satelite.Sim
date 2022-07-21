using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipseSphereDisplacer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineWidth;
    [SerializeField] private Color _color;
    [SerializeField] private Vector2 _radiuses;
    [SerializeField] private int _pointCount;

    [SerializeField] private float _displacementSphereRadius;

    private void OnValidate()
    {
        DrawEllipse();
    }

    private void DrawEllipse()
    {
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.startColor = _color;
        _lineRenderer.endColor = _color;
        
        _lineRenderer.positionCount = _pointCount;

        for (int i = 0; i < _pointCount; i++)
        {
            float angle = i * 2 * Mathf.PI / _pointCount;
            _lineRenderer.SetPosition(i, GetEllipsePoint(angle));
        }
        
        for (int i = 0; i < _pointCount; i++)
        {
            Vector3 direction = _lineRenderer.GetPosition(i) - Vector3.forward * _displacementSphereRadius;
            Vector3 delta = direction - direction.normalized * _displacementSphereRadius;
            _lineRenderer.SetPosition(i, _lineRenderer.GetPosition(i) - delta);
        }
    }

    private Vector2 GetEllipsePoint(float angle)
    {
        float x = _radiuses.x * Mathf.Cos(angle);
        float y = _radiuses.y * Mathf.Sin(angle);
        return new Vector2(x, y);
    }
}
