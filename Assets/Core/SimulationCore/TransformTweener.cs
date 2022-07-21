using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransformTweener : MonoBehaviour
{
    [SerializeField] private float _defaultTweenTime = 1f;

    public void TweenTo(Transform target)
    {
        transform.DOMove(target.position, _defaultTweenTime);
        transform.DORotate(target.eulerAngles, _defaultTweenTime);
    }
}
