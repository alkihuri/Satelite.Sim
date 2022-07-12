using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intefaces;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

namespace Entities
{
    public class Moon : MonoBehaviour, IPlanet
    { 
        public void Spin()
        {
            transform.Rotate(0, Constants.SimulatedMoonAngularSpeed * Time.deltaTime, 0);
        }

        private void Update()
        {
            Spin();
        }
    }
}