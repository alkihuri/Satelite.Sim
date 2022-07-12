using UnityEngine.EventSystems;

namespace Intefaces
{
    public interface ISelectable : IPointerClickHandler
    {
        bool IsSelected { get; }
        
        void Select();
        void DeSelect();
    }
}