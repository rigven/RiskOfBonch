using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageSplash : AttackPhase
{
    private int attackNumber = 0;
    int firingLinesNumber = 10;
    float radius = 2f;
    float turnAngle;

    bool asynchronousShooting = false;
    int asynchronousShotNumber = 0;

    new void Start()
    {
        base.Start();
        cooldown = 2f;
        damage *= DifficultyController.difficulty;
        turnAngle = 360f / firingLinesNumber;
    }

    public void Attack()
    {
        if (canStartAttack)
        {
            attacking = true;
            canStartAttack = false;
        }

        if (attacking)
        {
            animator.SetTrigger("Hit");
            switch (attackNumber)
            {
                case 0:
                    SynchronousShot(0);
                    break;
                case 1:
                    SynchronousShot(turnAngle / 2);
                    break;
                case 2:
                    AsynchronousShot(1, 1, 0.05f);
                    break;
                case 3:
                    AsynchronousShot(-1, firingLinesNumber/2, 0.5f);
                    break;
            }
        }
    }
    private void SynchronousShot(float offset)
    {
        for (int num = 0; num < firingLinesNumber; num++)
        {
            float angle = num * turnAngle + offset;
            PushBullet(angle);
        }

        attacking = false;
        IncreaseAttackNumber();
        StartCoroutine(AttackCooldown(cooldown));
    }

    private void PushBullet(float angle)
    {
        Vector3 position = new Vector3(transform.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad), transform.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

        VariedBullet bullet = Instantiate(projectile, position, Quaternion.identity).GetComponent<VariedBullet>();
        bullet.damage = damage;
        bullet.gameObject.transform.parent = boss.transform;
        Vector2 velocity = (position - transform.position).normalized * bullet.projectileSpeed;
        bullet.GrowAndLaunch(velocity);
    }

    private void AsynchronousShot(int bypassDirection, int directionsNumber, float timeBetweenShots)
    {

        if (!asynchronousShooting)
        {
            asynchronousShooting = true;
            asynchronousShotNumber = 0;
        }
        else
        {
            if (Time.realtimeSinceStartup - lastShoot > timeBetweenShots)
            {
                float angle = asynchronousShotNumber * 360f / firingLinesNumber * bypassDirection;
                lastShoot = Time.realtimeSinceStartup;
                for (int i = 0; i < directionsNumber; i++)
                {
                    PushBullet(angle + 360f / directionsNumber * i);
                }
                asynchronousShotNumber++;
            }
            if (asynchronousShotNumber >= firingLinesNumber)
            {
                asynchronousShooting = false;
                attacking = false;
                IncreaseAttackNumber();
                StartCoroutine(AttackCooldown(cooldown * 2));
            }
        }
    }

    private void IncreaseAttackNumber()
    {
        if (attackNumber < 3)
        {
            attackNumber++;
        }
        else
        {
            attackNumber = 0;
        }
    }

}
