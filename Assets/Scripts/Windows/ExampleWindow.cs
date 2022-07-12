

public class ExampleWindow : BaseWindow
{
    
        
    public void OnClickExampleSecondWindow()
    {
        _windowsManager.OpenWindow<ExampleSecondWindow>();
    }
    
}
