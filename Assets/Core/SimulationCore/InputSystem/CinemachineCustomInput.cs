using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using Settings;

public class CinemachineCustomInput : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook _freeLook;
    [SerializeField] CinemachineCameraOffset _offset;
    [SerializeField] private float _min_zoom;
    [SerializeField] private float _max_zoom;
    [SerializeField] private float _transitionValue;
    
    public UnityEvent ZoomedIn = new UnityEvent();
    public UnityEvent ZoomedOut = new UnityEvent();
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

        var settingsCoeficcientVertical = PlayerPrefs.GetFloat(SeetingsKeys.SwiperSpeedVertical);
        var settingsCoeficcientHoriztontal = PlayerPrefs.GetFloat(SeetingsKeys.SwiperSpeedHorizontal);

        _freeLook.m_XAxis.Value += InputSystem.UniversalInput.Instance.HORIZONTAL * settingsCoeficcientHoriztontal;
        _freeLook.m_YAxis.Value += InputSystem.UniversalInput.Instance.VERTICAL * settingsCoeficcientVertical;

        if (!Input.GetMouseButton(0))
        {
            _freeLook.m_XAxis.m_MaxSpeed = 0;
            _freeLook.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            _freeLook.m_XAxis.m_MaxSpeed = 15 * settingsCoeficcientHoriztontal;
            _freeLook.m_YAxis.m_MaxSpeed = 1 * settingsCoeficcientVertical;
        }

        float offset = -InputSystem.UniversalInput.Instance.CAMERA_ZOOM * PlayerPrefs.GetFloat(SeetingsKeys.MouseWheelSpeed);
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
            ZoomedIn?.Invoke();
            _isClose = true;
        }
        else if (_offset.m_Offset.z < _transitionValue && _isClose)
        {
            ZoomedOut?.Invoke();
            _isClose = false;
        }
    }
}
