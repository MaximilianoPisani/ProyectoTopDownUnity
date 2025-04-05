using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    public Transform Target;
    public float DetectionRange = 5f;

    private bool _hasDetectedTarget = false;

    protected override void HandleMovement()
    {
        if (Target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, Target.position);

        if (distanceToTarget <= DetectionRange)
        {
            _hasDetectedTarget = true;
        }

        if (_hasDetectedTarget)
        {
            Vector2 direction = (Target.position - transform.position).normalized;
            _movement = direction;
        }
        else
        {
            _movement = Vector2.zero;
        }
    }
}