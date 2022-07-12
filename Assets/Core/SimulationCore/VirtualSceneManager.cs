using System;
using System.Linq;
using UnityEngine;

// Not real scenes, but switchable objects acting as ones

[Serializable]
public class VirtualScene
{
    public string Name = "New Scene";
    public GameObject[] SceneObjects;

    public bool IsEnabled { get; private set; }

    public void Enable()
    {
        if (IsEnabled) return;
        foreach (var s in SceneObjects)
        {
            s.SetActive(true);
        }
        IsEnabled = true;
    }
    
    public void Disable()
    {
        if (IsEnabled == false) return;
        foreach (var s in SceneObjects)
        {
            s.SetActive(false);
        }
        IsEnabled = false;
    }
}

public class VirtualSceneManager : MonoSinglethon<VirtualSceneManager>
{
    [SerializeField] private VirtualScene[] _scenes;

    private void Start()
    {
        ChangeScene(_scenes[0].Name);
    }

    public void ChangeScene(string sceneName)
    {
        VirtualScene targetScene = _scenes.FirstOrDefault(s => s.Name == sceneName);
        if (ReferenceEquals(targetScene, null)) return;
        if (targetScene.IsEnabled) return;
        
        foreach (var s in _scenes)
        {
            s.Disable();
        }
        targetScene.Enable();
    }
}
