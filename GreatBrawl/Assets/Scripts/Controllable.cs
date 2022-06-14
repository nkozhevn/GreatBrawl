using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MobileTestProject
{
    public class Controllable : MonoBehaviour
    {
        [SerializeField] private Vector3 _positionOffset = new Vector3(0f, 0.005f, 0f);

        private Unit _unit;
        private LineRenderer _line;
        private Transform _pointer;
        private Unit _target;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void Update()
        {
            if (_pointer)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                plane.Raycast(ray, out var distance);
                Vector3 point = ray.GetPoint(distance);
                
                if (Input.GetMouseButtonUp(0))
                {
                    if(_target)
                        _unit.SetDestination(_target);
                    else
                    {
                        _unit.SetDestination(point);
                    }
                    
                    Destroy(_line.gameObject);
                    _line = null;
                    Destroy(_pointer.gameObject);
                    _pointer = null;
                    return;
                }
                
                _target = null;

                Physics.Raycast(ray, out var hit);
                if (hit.collider)
                {
                    var target = hit.collider.gameObject.GetComponent<Unit>();
                    if (target)
                    {
                        if (target.team == ETeam.Hostile)
                        {
                            _pointer.position = target.transform.position;
                            _target = target;
                        }
                    }
                }
                if(!_target)
                    _pointer.position = point;
                
                _line.SetPositions(new [] { transform.position + _positionOffset, _pointer.position + _positionOffset } );
            }
        }

        private void OnMouseDown()
        {
            _line = ResourceManager.CreatePrefabInstance<EElements, LineRenderer>(EElements.Line);
            _pointer = ResourceManager.CreatePrefabInstance<EElements, Transform>(EElements.Pointer);
        }
    }
}

