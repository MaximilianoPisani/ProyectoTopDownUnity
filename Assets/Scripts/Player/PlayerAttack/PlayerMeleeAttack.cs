using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : AttackMelee
{
    private bool _bufferedAttack = false;
    private float _bufferTime = 0.1f;
    private float _bufferTimer = 0f;

    private bool _isAlive = true;
    private void Update()
    {
        if (!_isAlive) return;

        float lx = animHandler.Animator.GetFloat("LastX");
        float ly = animHandler.Animator.GetFloat("LastY");
        lastLookDirection = new Vector2(lx, ly).normalized;

        UpdateAttackPointPosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (HasWeaponEquipped())
            {
                TryAttack();
            }
            else
            {
                _bufferedAttack = true;
                _bufferTimer = _bufferTime;
            }
        }

        if (_bufferedAttack)
        {
            _bufferTimer -= Time.deltaTime;

            if (_bufferTimer <= 0f)
            {
                _bufferedAttack = false;
            }
            else if (HasWeaponEquipped())
            {
                TryAttack();
                _bufferedAttack = false;
            }
        }
    }
    public void ResetAttackInput()
    {
        _bufferedAttack = false;
        _bufferTimer = 0f;

        if (animHandler != null)
        {
            animHandler.SetBool("isAttacking", false);
        }
    }

    private bool HasWeaponEquipped()
    {
        return GetComponent<WeaponManager>().currentWeapon != null;
    }

    public override void ApplyDamage()
    {
        base.ApplyDamage();
    }
}
