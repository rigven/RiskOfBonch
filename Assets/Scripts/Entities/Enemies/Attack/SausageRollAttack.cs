using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageRollAttack : Melee
{
    public override void Attack()
    {
        if (canAttack)
        {
            enemy.anim.SetTrigger("Attack");
        }
        base.Attack();
    }
}
