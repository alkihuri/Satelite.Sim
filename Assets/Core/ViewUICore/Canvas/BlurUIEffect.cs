using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace View.Canvas
{
    public class BlurUIEffect : UniversalUIElementEffect
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [FormerlySerializedAs("_graphics")] [SerializeField] private Graphic[] _bluredGraphics;

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
                    graphic.material.DOKill();
                    graphic.material.DOFloat(1.5f, "_BlurAmount", time);
                }
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(1f, time);
            }
            else
            {
                foreach (var graphic in _bluredGraphics)
                {
                    graphic.material.DOKill();
                    graphic.material.DOFloat(0, "_BlurAmount", time);
                }
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(0, time).OnComplete(Disable);
            }
        }
    }
}