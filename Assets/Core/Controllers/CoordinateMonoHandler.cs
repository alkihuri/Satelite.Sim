using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataControllers
{
    public class CoordinateHandlerForSatellite : MonoSinglethon<CoordinateHandlerForSatellite>
    {
        private Vector2 _2D;
        private List<Vector2> _markersOnMap = new List<Vector2>();
        private List<int> _gapIndexes = new List<int>();

        public Vector2 VALUE_2D => _2D;
        public List<Vector2> MARKERS_ON_MAP_2D => _markersOnMap;
        public List<int> GAP_INDEXES => _gapIndexes;

        public event Action CoordinatesUpdated;
        
        public void UPDATE_MARKERS(List<Vector2> points, List<int> gapIndexes, Vector2 currentPosition)
        {
            _markersOnMap.Clear();
            _2D = currentPosition;
            _markersOnMap = points;
            _gapIndexes = gapIndexes;
            CoordinatesUpdated?.Invoke();
        }
    }

}