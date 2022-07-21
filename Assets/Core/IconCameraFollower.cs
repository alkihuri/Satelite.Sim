using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IconCameraFollower : MonoBehaviour
{
    [SerializeField] private bool _follow = true;
    [SerializeField] private bool _changeSize = true;
    
    private Transform _target;

    private void Start()
    {
        _target = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // if (_follow) transform.LookAt(_target);
        if (_follow) transform.rotation = _target.rotation * Quaternion.Euler(0, 180, 0);
        if (_changeSize) transform.localScale = Vector3.one * (_target.position - transform.position).magnitude;
    }
}
