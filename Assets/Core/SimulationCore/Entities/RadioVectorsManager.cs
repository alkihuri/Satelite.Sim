using System.Collections.Generic;
using UnityEngine;

public class RadioVectorsManager : EntityManager
{
    [SerializeField] private bool _generateOnStart;
    [SerializeField] private LineRenderer _vectorPrefab;
    [SerializeField] private Vector2 _lengthRange;
    [SerializeField] private Color[] _randomColors;
    [SerializeField] private int _numOfVectors = 100;
    [SerializeField] private List<Transform> _vectorsList = new List<Transform>();
    
    private void Start()
    {
        if (_generateOnStart) InitPoolOfRadioVectors();
    }

    private void InitPoolOfRadioVectors()
    {
        for (int i = 0; i < _numOfVectors; i++)
        {
            Vector3 randomRotation = new Vector3(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
            );
            float randomLength = Random.Range(_lengthRange.x, _lengthRange.y);
            Color randomColor = _randomColors[Random.Range(0, _randomColors.Length)];

            LineRenderer radioVector = Instantiate(_vectorPrefab,
                transform.position,
                Quaternion.Euler(randomRotation),
                transform
            );
            radioVector.name = "RadioVector";

            radioVector.positionCount = 2;
            radioVector.SetPosition(0, Vector3.zero);
            radioVector.SetPosition(1, Vector3.one * randomLength);

            radioVector.startColor = randomColor;
            radioVector.endColor = randomColor;
            
            _vectorsList.Add(radioVector.transform);
        }
    }
    
    public override void ShowAmountOfEntities(float n)
    {
        float amount = n * _numOfVectors;
            
        for (int x = 0; x < _vectorsList.Count; x++)
        {
            var state = amount > x;
            if (_vectorsList[x].gameObject.activeSelf != state)
                _vectorsList[x].gameObject.SetActive(state);
        }
    }
}
