using UnityEngine;
using Zenject;

public class ExampleScript : MonoBehaviour
{
    [Inject] 
    private WindowsManager _windowsManager;
    
    public void OnClickExample()
    {
        _windowsManager.OpenWindow<ExampleWindow>();
    }
}
