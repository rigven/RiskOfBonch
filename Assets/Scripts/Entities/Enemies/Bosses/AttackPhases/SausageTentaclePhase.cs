using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageTentaclePhase : AttackPhase
{
    [SerializeField] SausageSpawner sausageSpawner;

    new void Start()
    {
        base.Start();
        cooldown = 15f;
    }

    public void Attack(int numberOfTentacles)
    {
        if (canStartAttack)
        {
            ReleaseTentacles(numberOfTentacles);
	    }
    }

    private void ReleaseTentacles(int numberOfTentacles)
    {
        for (int i = 0; i < numberOfTentacles; i++)
        {
            Instantiate(sausageSpawner, transform);
        }
        canStartAttack = false;
        StartCoroutine(AttackCooldown(cooldown));
    }
}
