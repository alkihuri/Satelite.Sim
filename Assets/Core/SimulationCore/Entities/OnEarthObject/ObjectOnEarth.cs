using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("_videPath")] [SerializeField] private string _videoPath;

        public bool IsSelected { get; }
        public void Select()
        {
            //_videoController.LoadVideo(_clip);
            _videoController.LoadVideoStreamingAssets(_videoPath, "");
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