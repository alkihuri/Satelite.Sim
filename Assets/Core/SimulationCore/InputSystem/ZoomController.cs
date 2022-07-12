using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputSystem
{
    public class ZoomController : MonoBehaviour
    {
        [SerializeField, Range(-1, 1)] private float _vertical;
        [SerializeField, Range(-1, 1)] private float _horizontal;
        [SerializeField, Range(-1, 1)] private float _zoom;
        [SerializeField] private Vector2 _sensitivity;
        [SerializeField] private float _zoomSensitivity;
        [SerializeField] private float _zoomDeadzone;
        [SerializeField] private int _smoothing;
        [SerializeField, Range(0, 1)] private float _moveInertia;
        [SerializeField, Range(0, 1)] private float _zoomInertia;
        
        private float _currentPinchValue;
        private float _prevPinchValue;
        
        private Queue<Touch> _firstFingerTouches;
        private Vector2 _averageFirstTouch;
        private Queue<Touch> _secondFingerTouches;
        private Vector2 _averageSecondTouch;

        private void Awake()
        {
            _firstFingerTouches = new Queue<Touch>();
            _secondFingerTouches = new Queue<Touch>();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                TouchHandler();
                InputHandler();
                
                if (Input.touchCount > 1) PinchFeature();
                else
                {
                    DampenZoom();
                    _currentPinchValue = 0;
                    _prevPinchValue = 0;
                }
            }
            else
            {
                DampenMove();
                DampenZoom();
                
                _firstFingerTouches = new Queue<Touch>();
                _secondFingerTouches = new Queue<Touch>();
            }
        }

        private void DampenMove()
        {
            UniversalInput.Instance.VERTICAL *= _moveInertia;
            UniversalInput.Instance.HORIZONTAL *= _moveInertia;
        }

        private void DampenZoom()
        {
            UniversalInput.Instance.CAMERA_ZOOM *= _zoomInertia;
        }

        private void PinchFeature()
        {
            _currentPinchValue = Vector2.Distance(_averageFirstTouch, _averageSecondTouch);
            if (_prevPinchValue == 0) _prevPinchValue = _currentPinchValue;
            
            float pinchDelta = _prevPinchValue - _currentPinchValue;
            if (Mathf.Abs(pinchDelta) > _zoomDeadzone)
                _zoom = pinchDelta * _zoomSensitivity;
            else _zoom = 0;

            UniversalInput.Instance.CAMERA_ZOOM = _zoom;
            _prevPinchValue = _currentPinchValue;
        }

        private void InputHandler()
        {
            var xDelta = _firstFingerTouches.Average(t => t.deltaPosition.x) * _sensitivity.x;
            UniversalInput.Instance.HORIZONTAL = Input.touchCount == 1 ? xDelta : 0;
            var yDelta = -_firstFingerTouches.Average(t => t.deltaPosition.y) * _sensitivity.y;
            UniversalInput.Instance.VERTICAL = Input.touchCount == 1 ? yDelta : 0;

            _vertical = UniversalInput.Instance.VERTICAL;
            _horizontal = UniversalInput.Instance.HORIZONTAL;
        }

        private void TouchHandler()
        {
            if (!Input.touchSupported)
                return;
            if (Input.touchCount > 0)
            {
                _firstFingerTouches.Enqueue(Input.GetTouch(0));
                if (_firstFingerTouches.Count > _smoothing) _firstFingerTouches.Dequeue();
                _averageFirstTouch = new Vector2(_firstFingerTouches.Average(t => t.position.x),
                    _firstFingerTouches.Average(t => t.position.y));
            }

            if (Input.touchCount > 1)
            {
                _secondFingerTouches.Enqueue(Input.GetTouch(1));
                if (_secondFingerTouches.Count > _smoothing) _secondFingerTouches.Dequeue();
                _averageSecondTouch = new Vector2(_secondFingerTouches.Average(t => t.position.x),
                    _secondFingerTouches.Average(t => t.position.y));
            }
        }
        
        private bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}