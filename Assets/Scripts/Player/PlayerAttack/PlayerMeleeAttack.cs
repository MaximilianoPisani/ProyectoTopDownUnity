using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : AttackMelee
{
    private bool _bufferedAttack = false;
    private float _bufferTime = 0.1f;
    private float _bufferTimer = 0f;

    private PlayerAttackController _attackController;
    private WeaponManager _weaponManager;

    private void Start()
    {
        _attackController = GetComponent<PlayerAttackController>();
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (!_isAlive) return;

        float lx = animHandler.Animator.GetFloat("LastX");
        float ly = animHandler.Animator.GetFloat("LastY");
        lastLookDirection = new Vector2(lx, ly).normalized;

        UpdateAttackPointPosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (HasMeleeWeaponEquipped())
            {
                if (_attackController.CanAttack(AttackType.Melee))
                {
                    TryAttack();
                }
                else
                {
                    _bufferedAttack = true;
                    _bufferTimer = _bufferTime;
                }
            }
        }

        if (_bufferedAttack)
        {
            _bufferTimer -= Time.deltaTime;
            if (_bufferTimer <= 0f)
            {
                _bufferedAttack = false;
            }
            else if (_attackController.CanAttack(AttackType.Melee))
            {
                TryAttack();
                _bufferedAttack = false;
            }
        }
    }

    private bool HasMeleeWeaponEquipped()
    {
        return _weaponManager != null && _weaponManager.meleeWeapon != null;
    }

    public override void TryAttack()
    {
        if (!canAttack || !_isAlive) return;
        if (_attackController.isAttacking && _attackController.currentAttack == AttackType.Melee) return;
        if (!IsCooldownReady()) return;

        base.TryAttack();
        _attackController.StartAttack(AttackType.Melee);
    }


    public void OnMeleeAnimationEnd()
    {
        _attackController.EndAttack(AttackType.Melee);
    }

    public override void ApplyDamage()
    {
        base.ApplyDamage();
    }
}