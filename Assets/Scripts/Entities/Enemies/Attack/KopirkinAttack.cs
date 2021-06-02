using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopirkinAttack : Range
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
            base.Attack();
            enemy.anim.SetTrigger("Attack");
        }
    }
}
