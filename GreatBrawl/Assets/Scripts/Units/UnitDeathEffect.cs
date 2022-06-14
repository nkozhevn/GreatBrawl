using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTestProject
{
    public class UnitDeathEffect : MonoBehaviour
    {
        [SerializeField] private float effectLength;
        
        private const string die = "Die";
        private Animator _animator;
    
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _animator.SetTrigger(die);
            Destroy(gameObject, effectLength);
        }
    }
}

