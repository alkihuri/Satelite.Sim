using UnityEngine;
using DG.Tweening;


namespace View.Canvas
{

    public class UniversalUIElementEffect : MonoBehaviour
    {

        private RectTransform _rectTransform;
        
        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetActive(bool state, float time = 0.2f)
        {
            if (gameObject.activeSelf == state) return;
            
            if (state)
            {
                Enable(); 
                _rectTransform.DOScale(1, time);
            }
            else
                _rectTransform.DOScale(0, time).OnComplete(Disable);
        }

        public void SetActive(bool state)
        {
            SetActive(state, 0.2f);
        }

        protected void Disable()
        {
            gameObject.SetActive(false);
        }
        protected void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}