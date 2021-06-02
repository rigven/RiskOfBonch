using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSphereAttack : Melee
{
    private ParticleSystem electricShock;

    new protected void Start()
    {
        base.Start();
        electricShock = transform.Find("ElectricShock").GetComponent<ParticleSystem>();
    }

    public override void Attack()
    {
        if (canAttack)
        {
            electricShock.transform.right = (enemy.targetPlayer.transform.position - transform.position).normalized;
            electricShock.Play();
        }
        base.Attack();
    }
}
