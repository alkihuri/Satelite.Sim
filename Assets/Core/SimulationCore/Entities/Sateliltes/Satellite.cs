using UnityEngine;
using Intefaces;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

namespace Entities
{
    public enum OrbitType
    {
        Random,
        Low,
        LowEquator,
        Medium,
        Geostationary
    }

    public class Satellite : MonoBehaviour, ISatelite, ISelectable
    {
        [Header("Satelite main info")]
        [SerializeField] SatelliteInfoSO _info;
        
        [Header("Satelite settings")]
        [SerializeField] private GameObject _earth;
        [SerializeField] private GameObject _shape;
        [SerializeField] private GameObject _cinemachine;
        [SerializeField] private GameObject _detailedMesh;
        [SerializeField] private GameObject _simpleMesh;
        private bool _interactable;
        private RaycastHit _hitInfo;
        private Vector3 _hitPoint;

        [Header("Events")]
        [SerializeField] public UnityEvent OnSelect = new UnityEvent();
        [SerializeField] public UnityEvent OnDeselect = new UnityEvent();
        private bool _selected;

        public bool IsSelected => _selected;

        private void Start()
        {
            CashComponents();
            TurnOffCinemachine();
        }

        private void CashComponents()
        {
            _selected = false;
            _earth = GetComponentInParent<Earth>().gameObject;
        }

        public void Select()
        {
            if (IsSelected == false)
            {
                if (SelectionManager.Instance.TrySelect(this) == false) return;
                
                SelectionManager.Instance.IsLocked = true;
                
                SelectebleObjectInfo.Instance.ApplyInfo(_info);
                _selected = true;
                TurnOnCinemachine();
                CameraSettings.CameraController.Instance.SwitchCamera(gameObject);
                ShowMesh();
                OnSelect?.Invoke();
            }
        }
        public void DeSelect()
        {
            if (IsSelected)
            {
                TurnOffCinemachine();
                _selected = false;
                CameraSettings.CameraController.Instance.SwitchCamera(_earth);
                HideMesh();
                OnDeselect?.Invoke();
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_selected)
                Select();
            // else
            //     DeSelect();
        }
        
        public void DoBig()
        {
            _shape.transform.DOScale(Vector3.one * 0.15f, 1);
        }
        public void DoSmall()
        {
            _shape.transform.DOScale(Vector3.one * 0.10f, 1);
        }

        public void TurnOnCinemachine()
        {
            _cinemachine.SetActive(true);
        }
        public void TurnOffCinemachine()
        {
            _cinemachine.SetActive(false);
        }

        public void ShowMesh()
        {
            _detailedMesh.SetActive(true);
            _simpleMesh.SetActive(false);
        }
        public void HideMesh()
        {
            _detailedMesh.SetActive(false);
            _simpleMesh.SetActive(true);
        }
    }
}