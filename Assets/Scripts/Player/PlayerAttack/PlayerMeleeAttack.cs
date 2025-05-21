using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : AttackMelee
{
    
    private void Update()
    {
        float lx = animHandler.Animator.GetFloat("LastX");
        float ly = animHandler.Animator.GetFloat("LastY");
        lastLookDirection = new Vector2(lx, ly).normalized;

        UpdateAttackPointPosition();

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }


    public override void ApplyDamage() // ApplyDamage to apply damage in specific frames only (Animations + Events).
    {
        base.ApplyDamage(); 
    }
}