using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None,
    Melee,
    Ranged
}

public class PlayerAttackController : MonoBehaviour
{
    public bool isAttacking;
    public AttackType currentAttack = AttackType.None;
    private AttackType _bufferedAttack = AttackType.None;

    private AnimationControllerHandler _animHandler;

    private void Awake()
    {
        _animHandler = GetComponent<AnimationControllerHandler>();
        if (_animHandler == null)
        {
            Debug.LogError("Missing AnimationControllerHandler on Player");
        }
    }

    public void StartAttack(AttackType type)
    {
        if (isAttacking)
        {
            _bufferedAttack = type;
            return;
        }

        isAttacking = true;
        currentAttack = type;
        Debug.Log($"Start attack: {type}");

        if (_animHandler != null)
        {
            if (type == AttackType.Melee)
                _animHandler.SetBool("isAttackingMelee", true);
            else if (type == AttackType.Ranged)
                _animHandler.SetBool("isAttackingRange", true);
        }
    }

    public void EndAttack(AttackType type)
    {
        if (currentAttack == type)
        {
            isAttacking = false;
            currentAttack = AttackType.None;
            Debug.Log($"End attack: {type}");


            if (_animHandler != null)
            {
                if (type == AttackType.Melee)
                    _animHandler.SetBool("isAttackingMelee", false);
                else if (type == AttackType.Ranged)
                    _animHandler.SetBool("isAttackingRange", false);
            }

            if (_bufferedAttack != AttackType.None)
            {
                AttackType next = _bufferedAttack;
                _bufferedAttack = AttackType.None;
                StartAttack(next);
            }
        }
    }

    public bool CanAttack(AttackType type)
    {
        return !isAttacking || (currentAttack == type);
    }
}