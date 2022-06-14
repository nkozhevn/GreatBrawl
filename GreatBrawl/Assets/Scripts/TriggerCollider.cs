using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MobileTestProject
{
    public class TriggerCollider : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<Collider> onTriggerEnter;
        [HideInInspector] public UnityEvent<Collider> onTriggerExit;

        private List<Collider> _colliders = new List<Collider>();

        private void OnTriggerEnter(Collider col)
        {
            _colliders.Add(col);
            onTriggerEnter.Invoke(col);
        }

        private void OnTriggerExit(Collider col)
        {
            _colliders.Remove(col);
            onTriggerExit.Invoke(col);
        }

        public List<Collider> GetContacts() => _colliders;
    }
}

