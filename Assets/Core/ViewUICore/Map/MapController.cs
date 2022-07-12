using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using DataControllers;

public class MapController : MonoBehaviour
{
    [SerializeField] private RectTransform _map;
    [SerializeField] private RectTransform _currentSatelite;
    [SerializeField] private Vector2 _dedictaionPoint;
    [SerializeField] private UILineRenderer _uiLineRenderer;
    
    private float _rectHeight;
    private float _rectWidth;

    void Start()
    {
        _rectHeight = _map.rect.height;
        _rectWidth = _map.rect.width;
        _dedictaionPoint = new Vector2(0, 0);
    }

    private void OnEnable()
    {
        CoordinateHandlerForSatellite.Instance.CoordinatesUpdated += SyncCoordinates;
    }
    
    private void OnDisable()
    {
        CoordinateHandlerForSatellite.Instance.CoordinatesUpdated -= SyncCoordinates;
    }

    private void SyncCoordinates()
    {
        SyncSatellitePosition();
        SyncSatellitePathPosition();
    }

    private void SyncSatellitePathPosition()
    {
        List<Vector2> satellitePathFromModel = CoordinateHandlerForSatellite.Instance.MARKERS_ON_MAP_2D;
        satellitePathFromModel = satellitePathFromModel
            .Select(p => new Vector2(p.x * _rectWidth, p.y * _rectHeight)).ToList();
        
        _uiLineRenderer.SetPositions(satellitePathFromModel, CoordinateHandlerForSatellite.Instance.GAP_INDEXES);
    }

    private void SyncSatellitePosition()
    {
        _dedictaionPoint = CoordinateHandlerForSatellite.Instance.VALUE_2D;
        _dedictaionPoint = new Vector2(_dedictaionPoint.x * _rectWidth, _dedictaionPoint.y * _rectHeight);
        _currentSatelite.DOAnchorPos(_dedictaionPoint, 0.1f);
    }
}
