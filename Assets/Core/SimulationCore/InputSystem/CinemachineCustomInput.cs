using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CinemachineCustomInput : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook _freeLook;
    [SerializeField] CinemachineCameraOffset _offset;
    [SerializeField] private float _min_zoom;
    [SerializeField] private float _max_zoom;
    [SerializeField] private float _transitionValue;
    
    public UnityEvent<bool> ZoomedIn = new UnityEvent<bool>();
    public UnityEvent<bool> ZoomedOut = new UnityEvent<bool>();
    private float _yMaxSpeed;
    private float _xMaxSpeed;
    private bool _isClose;
    
    private void Start()
    {
        _freeLook = GetComponent<CinemachineFreeLook>();
        _offset = GetComponent<CinemachineCameraOffset>();
        _yMaxSpeed = _freeLook.m_YAxis.m_MaxSpeed;
        _xMaxSpeed = _freeLook.m_XAxis.m_MaxSpeed;
        CheckCloseness();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedSetup();

        if (!CustomInputStateStatic.InputIsActivated)
        {
            return;
        }


        _freeLook.m_XAxis.Value += InputSystem.UniversalInput.Instance.HORIZONTAL;
        _freeLook.m_YAxis.Value += InputSystem.UniversalInput.Instance.VERTICAL;

        if (!Input.GetMouseButton(0))
        {
            _freeLook.m_XAxis.m_MaxSpeed = 0;
            _freeLook.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            _freeLook.m_XAxis.m_MaxSpeed = 15;
            _freeLook.m_YAxis.m_MaxSpeed = 1;
        }

        float offset = -InputSystem.UniversalInput.Instance.CAMERA_ZOOM;
        offset += _offset.m_Offset.z;
        _offset.m_Offset.z = Mathf.Clamp(offset, _min_zoom, _max_zoom);

        CheckCloseness();
    }

    private void SpeedSetup()
    {
        _freeLook.m_XAxis.m_MaxSpeed =
            CustomInputStateStatic.InputIsActivated ? _xMaxSpeed : 0;

        _freeLook.m_YAxis.m_MaxSpeed =
            CustomInputStateStatic.InputIsActivated ? _yMaxSpeed : 0;
    }

    private void CheckCloseness()
    {
        if (_offset.m_Offset.z > _transitionValue && _isClose == false)
        {
            ZoomedIn?.Invoke(true);
            _isClose = true;
        }
        else if (_offset.m_Offset.z < _transitionValue && _isClose)
        {
            ZoomedOut?.Invoke(false);
            _isClose = false;
        }
    }
}
