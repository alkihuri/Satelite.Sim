using Intefaces;

public class SelectionManager : MonoSinglethon<SelectionManager>
{
    public bool IsLocked { get; set; }
    
    private ISelectable _currentSelection;

    public bool IsAnySelected => _currentSelection != null;

    public bool TrySelect(ISelectable selectable)
    {
        if (IsLocked) return false;

        _currentSelection?.DeSelect();
        _currentSelection = selectable;
        return true;
    }

    public bool TryDeselectCurrent()
    {
        if (IsLocked) return false;
        
        _currentSelection?.DeSelect();
        return true;
    }
    
    public void DeselectCurrent()
    {
        if (IsLocked) return;
        
        _currentSelection?.DeSelect();
        _currentSelection = null;
    }
}
