using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    [SerializeField] private Gradient _gradient;
    [SerializeField] private List<Vector2> _positions = new List<Vector2>();
    [SerializeField] private float _thickness = 10f;
    
    private List<int> _gapIndexes;

    public void SetPositions(List<Vector2> positions, List<int> gapIndexes = null)
    {
        _positions = positions;
        _gapIndexes = gapIndexes;
        SetAllDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (_positions.Count < 2) return;
        
        for (int i = 0; i < _positions.Count; i++)
        {
            Vector2 direction;
            if (i == 0 || _gapIndexes.Contains(i - 1))
                direction = _positions[i + 1] - _positions[i];
            else if (i < _positions.Count - 1 && _gapIndexes.Contains(i) == false)
                direction = -(_positions[i - 1] - _positions[i]).normalized + (_positions[i + 1] - _positions[i]).normalized;
            else
                direction = _positions[i] - _positions[i - 1];
            
            DrawVerticesForPosition(_positions[i], vh, direction, _gradient.Evaluate((float)i / _positions.Count));
        }
        
        for (int i = 0; i < _positions.Count - 1; i++)
        {
            if (_gapIndexes != null)
                if (_gapIndexes.Contains(i)) continue;
            
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
    }

    private void DrawVerticesForPosition(Vector2 position, VertexHelper vh, Vector2 direction, Color32 vertexColor)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = vertexColor;
        direction = direction.normalized;


        vertex.position = Quaternion.LookRotation(Vector3.forward, direction) * new Vector3(-_thickness / 2, 0);
        vertex.position += new Vector3(position.x, position.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.LookRotation(Vector3.forward, direction) * new Vector3(_thickness / 2, 0);
        vertex.position += new Vector3(position.x, position.y);
        vh.AddVert(vertex);
    }
}
