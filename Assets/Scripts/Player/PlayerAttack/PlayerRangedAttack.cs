using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : AttackRanged //#TEST
{
    private WeaponManager _weaponManager;
    [SerializeField] private RangedAttackData _attackData;

    protected override void Awake()
    {
        base.Awake();
        _weaponManager = GetComponent<WeaponManager>();
        if (_weaponManager == null)
            Debug.LogWarning("WeaponManager not found on player.");

        rangedAttackData = _attackData;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && IsCurrentWeaponRanged())
        {
            TryAttack();
        }

    }

    public override void TryAttack()
    {
        if (!canAttack || !isAlive) return;

        canAttack = false;
        animHandler.SetBool("isAttackingRange", true);

        ShootCardinal(); 

        Invoke(nameof(ResetAttack), rangedAttackData.Cooldown);
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

        Shoot(); 
    }

    public void ResetAttackInput()
    {
        if (animHandler != null)
        {
            animHandler.SetBool("isAttackingRange", false);
        }

        CancelInvoke(nameof(ResetAttack));
        canAttack = true;
    }

    private bool IsCurrentWeaponRanged()
    {
        if (_weaponManager == null || _weaponManager.currentWeapon == null) return false;

        Weapon currentWeaponScript = _weaponManager.currentWeapon.GetComponent<Weapon>();
        if (currentWeaponScript == null) return false;

        return currentWeaponScript.weaponName == "Shotgun";
    }
}