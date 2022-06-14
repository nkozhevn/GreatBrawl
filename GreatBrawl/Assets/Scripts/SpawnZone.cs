using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTestProject
{
    public class SpawnZone : MonoBehaviour
    {
        [HideInInspector] public float minX;
        [HideInInspector] public float maxX;
        [HideInInspector] public float minZ;
        [HideInInspector] public float maxZ;

        private void Awake()
        {
            minX = transform.position.x - transform.localScale.x / 2;
            maxX = transform.position.x + transform.localScale.x / 2;
            minZ = transform.position.z - transform.localScale.z / 2;
            maxZ = transform.position.z + transform.localScale.z / 2;
        }
    }
}

