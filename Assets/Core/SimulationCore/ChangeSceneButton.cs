using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ChangeScene);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(ChangeScene);
    }

    private void ChangeScene()
    {
        SelectionManager.Instance.IsLocked = false;
        SelectionManager.Instance.DeselectCurrent();
        VirtualSceneManager.Instance.ChangeScene(_sceneName);
    }
}
