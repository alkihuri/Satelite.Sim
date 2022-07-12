using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace Entities.View
{
    public class EntitieView : MonoBehaviour
    {
        [SerializeField] private GameObject _interactableVisual;
        [SerializeField] private GameObject _nonInteractableVisual;
        [FormerlySerializedAs("_satelite")] [SerializeField] Satellite satellite;
        private void Start()
        {
            satellite = GetComponent<Satellite>();
            // SetViewInteractableState(satellite.INTERACTABLE);
        } 
        private void SetViewInteractableState(bool state)
        {
            _interactableVisual.SetActive(state);
            _nonInteractableVisual.SetActive(!state);
        }
    }
}