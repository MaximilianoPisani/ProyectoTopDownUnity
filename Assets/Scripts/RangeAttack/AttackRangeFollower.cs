using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private float offsetDistance = 0.5f; 

    private Vector2 lastDirection = Vector2.right;

    private void Update()
    {
        if (animator == null || player == null)
            return;

        float lx = animator.GetFloat("LastX");
        float ly = animator.GetFloat("LastY");

        Vector2 direction = new Vector2(lx, ly);
        if (direction.sqrMagnitude > 0.01f)
            lastDirection = direction.normalized;

        transform.position = (Vector2)player.position + lastDirection * offsetDistance;
    }
}