﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace View.Canvas
{
    public class BlurUIEffect : UniversalUIElementEffect
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [FormerlySerializedAs("_graphics")] [SerializeField] private Graphic[] _bluredGraphics;
        [SerializeField] private float _maxBlur = 1.5f;
        
        private static readonly int BlurAmount = Shader.PropertyToID("_BlurAmount");

        protected override void Awake()
        {
            if (!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();

            foreach (var graphic in _bluredGraphics)
            {
                graphic.material = Instantiate(graphic.material);
            }
            
            base.Awake();
        }

        public override void SetActive(bool state, float time = 0.2f)
        {
            if (gameObject.activeSelf == state) return;
            
            if (state)
            {
                Enable();
                foreach (var graphic in _bluredGraphics)
                {
                    if (graphic.material.DOKill() == 0)
                        graphic.material.SetFloat(BlurAmount, 0);
                    graphic.material.DOFloat(_maxBlur, BlurAmount, time);
                }

                if (_canvasGroup.DOKill() == 0)
                    _canvasGroup.alpha = 0;
                _canvasGroup.DOFade(1f, time);
            }
            else
            {
                foreach (var graphic in _bluredGraphics)
                {
                    if (graphic.material.DOKill() == 0)
                        graphic.material.SetFloat(BlurAmount, _maxBlur);
                    graphic.material.DOFloat(0, BlurAmount, time);
                }
                if (_canvasGroup.DOKill() == 0)
                    _canvasGroup.alpha = 1;
                _canvasGroup.DOFade(0, time).OnComplete(Disable);
            }
        }
    }
}