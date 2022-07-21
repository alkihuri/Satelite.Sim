using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.UI;

namespace View.Canvas.Video
{
    public class TransparentVideoController : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private bool _transparencySupport;
        [SerializeField] private VideoPlayer _videoPlayerColor;
        [SerializeField] private VideoPlayer _videoPlayerAlpha;
        [SerializeField] private Image _progressBar;

        [Header("Streming video settings:")]
        [SerializeField] string _colorPath;
        [SerializeField] string _alphaPath;

        private void Awake()
        {
            ClearTextures();
        }

        private void OnEnable()
        {
            LoadVideoStreamingAssets();
            StartVideo();
        }

        private void OnDisable()
        {
            Pause();
            ClearTextures();
        }

        private void Update()
        {
            UpdateProgress();
        }

        public void OnDrag(PointerEventData eventData)
        {
            TrySkip(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TrySkip(eventData);
        }

        private void SkipToPercent(float percent)
        {
            var frame = _videoPlayerColor.frameCount * percent;
            _videoPlayerColor.frame = (long)frame;

            if (!_transparencySupport) return;

            frame = _videoPlayerAlpha.frameCount * percent;
            _videoPlayerAlpha.frame = (long)frame;
        }

        private void TrySkip(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _progressBar.rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
            {
                float percent = Mathf.InverseLerp(_progressBar.rectTransform.rect.xMin, _progressBar.rectTransform.rect.xMax, localPoint.x);
                SkipToPercent(percent);
            }
        }


        private void ClearTextures()
        {
            RenderTexture backupTexture = RenderTexture.active;

            RenderTexture.active = _videoPlayerColor.targetTexture;
            GL.Clear(true, true, Color.black);

            if (_transparencySupport)
            {
                RenderTexture.active = _videoPlayerAlpha.targetTexture;
                GL.Clear(true, true, Color.black);
            }

            RenderTexture.active = backupTexture;
            // _videoPlayerColor.targetTexture.Release();
            // if (_transparencySupport) _videoPlayerAlpha.targetTexture.Release();
        }

        public void LoadVideo(VideoClip clipColor, VideoClip clipAlpha = null)
        {
            _videoPlayerColor.clip = clipColor;
            if (clipAlpha) _videoPlayerAlpha.clip = clipAlpha;
        }


        public void LoadVideoStreamingAssets(string colorPath, string alphaPath)
        {
            if (colorPath.Length > 0)
                _videoPlayerColor.url = Application.streamingAssetsPath + "/Videos/" + colorPath;
            if (alphaPath.Length > 0)
                _videoPlayerAlpha.url = Application.streamingAssetsPath + "/Videos/" + alphaPath;
        }

        public void LoadVideoStreamingAssets()
        {
            if (_colorPath.Length > 0)
                _videoPlayerColor.url = Application.streamingAssetsPath + "/Videos/" + _colorPath;
            if (_alphaPath.Length > 0)
                _videoPlayerAlpha.url = Application.streamingAssetsPath + "/Videos/" + _alphaPath;
        }

        public void StartVideo()
        {
            StartCoroutine(StartVideoRoutine());
        }

        public void Play()
        {
            if (_transparencySupport)
            {
                if (!_videoPlayerColor.isPrepared || !_videoPlayerAlpha.isPrepared) return;

                _videoPlayerColor.Play();
                _videoPlayerAlpha.Play();
            }
            else
            {
                if (!_videoPlayerColor.isPrepared) return;

                _videoPlayerColor.Play();
            }
        }

        public void Pause()
        {
            if (_transparencySupport)
            {
                if (!_videoPlayerColor.isPrepared || !_videoPlayerAlpha.isPrepared) return;

                _videoPlayerColor.Pause();
                _videoPlayerAlpha.Pause();
            }
            else
            {
                if (!_videoPlayerColor.isPrepared) return;

                _videoPlayerColor.Pause();
            }
        }

        public void SwitchPlay()
        {
            if (_videoPlayerColor.isPaused)
                Play();
            else
                Pause();
        }

        private IEnumerator StartVideoRoutine()
        {
            _videoPlayerColor.Prepare();
            if (_transparencySupport)
            {
                _videoPlayerAlpha.Prepare();
                yield return new WaitWhile(() => !_videoPlayerColor.isPrepared || !_videoPlayerAlpha.isPrepared);
                _videoPlayerAlpha.Play();
            }
            else
            {
                yield return new WaitWhile(() => !_videoPlayerColor.isPrepared);
            }
            _videoPlayerColor.Play();
        }

        public void PrepareVideo()
        {
            _videoPlayerColor.Prepare();
            _videoPlayerAlpha.Prepare();
        }

        private void UpdateProgress()
        {
            if (_videoPlayerColor.frameCount > 0)
                _progressBar.fillAmount = (float)_videoPlayerColor.frame / (float)_videoPlayerColor.frameCount;
        }
    }
}