using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    private Vector2 _startPosition;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _startPosition = transform.position;
    }


    void Update()
    {
        if (_target == null) return;

        _agent.SetDestination(_target.position);
    }

    public void SetTarget(Transform target)
    {

        _target = target;

        if (_target == null)
        {
            _agent.SetDestination(_startPosition);

        }
    }


}