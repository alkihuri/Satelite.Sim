using UnityEngine;
using UnityEngine.UI;
using View.Canvas.Video;

public class VideoPlayButton : MonoBehaviour
{
    [SerializeField] private TransparentVideoController[] _videos;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        foreach (TransparentVideoController video in _videos)
        {
            video.SwitchPlay();
        }
    }
}
