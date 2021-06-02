using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFlyingEnemy : FlyingEnemy
{
    new protected void Start()
    {
        base.Start();
    }

    new protected void Update()
    {
        if (!GamePause.isPaused)
        {
            base.Update();

            if (!wanders && enemy.targetPlayer)
            {
                TurnToPlayer();
            }
        }
    }

    private void TurnToPlayer()
    {
        Vector3 enemyRotation = (enemy.targetPlayer.transform.position - gameObject.transform.position);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, enemyRotation);
    }
}
