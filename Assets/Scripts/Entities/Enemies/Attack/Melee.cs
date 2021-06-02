using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : EnemyAttack
{
    new protected void Start()
    {
        base.Start();
    }

    new protected void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        if (canAttack)
        {
            enemy.targetPlayer.HandleDamage(damage);
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
    }
}
