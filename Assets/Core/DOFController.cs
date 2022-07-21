using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DOFController : MonoSinglethon<DOFController>
{
    [SerializeField] private Volume _volume;
    [SerializeField] private float _transitionTime = 1f;
    [SerializeField] private float _maxDOF = 50;

    private float _focalLengthValue;
    private DepthOfField _dof;

    protected override void Awake()
    {
        base.Awake();

        _volume.profile.TryGet(out _dof);
    }

    public void EnableDOF()
    {
        StopAllCoroutines();
        StartCoroutine(EnableDOFRoutine());
        
    }

    public void DisableDOF()
    {
        StopAllCoroutines();
        StartCoroutine(DisableDOFRoutine());
    }

    private IEnumerator EnableDOFRoutine()
    {
        float t = 0;
        while (t < 1)
        {
            _dof.focalLength.value = Mathf.Lerp(1, _maxDOF, t);
            t += Time.deltaTime / _transitionTime;
            yield return null;
        }
    }
    
    private IEnumerator DisableDOFRoutine()
    {
        float t = 0;
        while (t < 1)
        {
            _dof.focalLength.value = Mathf.Lerp(_maxDOF, 1, t);
            t += Time.deltaTime / _transitionTime;
            yield return null;
        }
    }
}
