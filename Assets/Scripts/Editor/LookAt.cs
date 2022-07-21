using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Vector3 _target;
    [SerializeField] private bool _inverted;

    [ContextMenu("Look At Target")]
    private void LookAtTarget()
    {
        Vector3 direction = _target - transform.position;
        transform.up = _inverted ? direction : -direction;
    }
}
