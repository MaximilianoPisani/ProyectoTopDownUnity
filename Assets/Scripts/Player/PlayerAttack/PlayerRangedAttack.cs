using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : AttackRanged
{
    private WeaponManager _weaponManager;
    private PlayerAttackController _attackController;

    [SerializeField] private RangedAttackData _attackData;

    private bool _bufferedAttack = false;
    private float _bufferTime = 0.1f;
    private float _bufferTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        _weaponManager = GetComponent<WeaponManager>();
        _attackController = GetComponent<PlayerAttackController>();

        if (_weaponManager == null)
            Debug.LogWarning("WeaponManager not found on player.");

        rangedAttackData = _attackData;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(1) && IsRangedWeaponEquipped())
        {
            if (_attackController.CanAttack(AttackType.Ranged) && canAttack)
            {
                TryAttack();
            }
            else if (!_attackController.CanAttack(AttackType.Ranged))
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
            else if (_attackController.CanAttack(AttackType.Ranged) && canAttack)
            {
                TryAttack();
                _bufferedAttack = false;
            }
        }
    }

    public override void TryAttack()
    {
        if (!canAttack || !isAlive) return;

        canAttack = false;
        _attackController.StartAttack(AttackType.Ranged);

        ShootCardinal();
        Shoot();

        Invoke(nameof(ResetAttack), rangedAttackData.Cooldown);
    }

    public void ResetAttackInput()
    {
        canAttack = true;
    }

    private void ShootCardinal()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inputDir == Vector2.zero)
        {
            inputDir = lastLookDirection;
        }

        if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y))
        {
            inputDir = inputDir.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            inputDir = inputDir.y > 0 ? Vector2.up : Vector2.down;
        }

        lastLookDirection = inputDir;
    }

    private bool IsRangedWeaponEquipped()
    {
        if (_weaponManager == null || _weaponManager.rangedWeapon == null) return false;

        Weapon current = _weaponManager.rangedWeapon.GetComponent<Weapon>();
        return current != null && current.weaponType == WeaponType.Ranged;
    }

    public void OnRangedAnimationEnd()
    {
        _attackController.EndAttack(AttackType.Ranged);
    }
}