using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using View.Canvas;
using View.Canvas.Video;
using ISelectable = Intefaces.ISelectable;

namespace Entities
{
    public class ObjectOnEarth : MonoBehaviour, ISelectable
    {
        [SerializeField] private TransparentVideoController _videoController;
        [SerializeField] private UniversalUIElementEffect _videoPanel;
        [SerializeField] private VideoClip _clip;

        public bool IsSelected { get; }
        public void Select()
        {
            _videoController.LoadVideo(_clip);
            _videoPanel.SetActive(true);
        }

        public void DeSelect()
        {
            return;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (SelectionManager.Instance.TrySelect(this))
            {
                Select();
                SelectionManager.Instance.IsLocked = true;
            }
        }
    }
}