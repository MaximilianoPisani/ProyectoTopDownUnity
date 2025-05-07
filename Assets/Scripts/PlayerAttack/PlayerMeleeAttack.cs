using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : AttackMelee
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }
}