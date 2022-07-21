using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class IconDrawer : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Vector2 _iconScaleRange;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private bool _adaptScale;
    
    public Transform[] Transforms;

    private uint _randomSeed;
    
    private void Start()
    {
        _randomSeed = (uint) Environment.TickCount;
    }

    private void Update()
    {
        if (Transforms.Length <= 0) return;
        DrawIcons();
    }

    private void DrawIcons()
    {
        var materialRandom = new Unity.Mathematics.Random(_randomSeed);
        var scaleRandom = new Unity.Mathematics.Random(_randomSeed);
        
        foreach (var t in Transforms)
        {
            if (!t.gameObject.activeInHierarchy)
            {
                continue;
            }

            float scale = scaleRandom.NextFloat(_iconScaleRange.x, _iconScaleRange.y);
            if (_adaptScale) scale *= (_lookTarget.position - t.position).magnitude;
            Matrix4x4 transformationMatrix = Matrix4x4.TRS(t.position, _lookTarget ? _lookTarget.rotation : t.rotation, Vector3.one * scale);
            Material material = _materials[materialRandom.NextInt(0, _materials.Length)];
            
            Graphics.DrawMesh(
                _mesh,
                transformationMatrix,
                material,
                gameObject.layer,
                null,
                0,
                null,
                ShadowCastingMode.Off,
                false,
                null,
                false
            );
        }
        
    }
}
