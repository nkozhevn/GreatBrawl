using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTestProject
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnZone spawnZone;
        
        private Unit _currentUnit;

        private void Awake()
        {
            spawnZone.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_currentUnit)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            plane.Raycast(ray, out var distance);
            Vector3 point = ray.GetPoint(distance);

            _currentUnit.transform.position = point;

            if (Input.GetMouseButtonUp(0))
            {
                if (point.x >= spawnZone.minX && point.x <= spawnZone.maxX && 
                    point.z >= spawnZone.minZ && point.z <= spawnZone.maxZ)
                {
                    _currentUnit.Place(ETeam.Allied, GameManager.HostileCore);
                    _currentUnit = null;
                }
                else
                {
                    Destroy(_currentUnit.gameObject);
                    _currentUnit = null;
                }
                spawnZone.gameObject.SetActive(false);
            }
        }
        
        public void Create(Unit unit)
        {
            spawnZone.gameObject.SetActive(true);
            _currentUnit = Instantiate(unit);
        }
    }
}

