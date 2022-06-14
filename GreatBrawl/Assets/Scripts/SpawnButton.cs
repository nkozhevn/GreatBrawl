using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTestProject
{
    public class SpawnButton : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private UnitSpawner spawner;

        private void Awake()
        {
            //GetComponent<Button>().OnPointerDown(() => { spawner.Create(unit);});
        }
    }
}

