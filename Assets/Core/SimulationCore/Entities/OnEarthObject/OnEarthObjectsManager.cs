using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.Canvas;

namespace Entities
{
    public class OnEarthObjectsManager : MonoBehaviour
    {
        public List<ObjectOnEarth> Objects = new List<ObjectOnEarth>();
        
        void Start()
        {
            Objects = GetComponentsInChildren<ObjectOnEarth>().ToList();
            HideObjects();
        }

        public void ShowObjects()
        {
            foreach (var obj in Objects)
            {
                obj.gameObject.SetActive(true);
            }
        }
        
        public void HideObjects()
        {
            foreach (var obj in Objects)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }
}