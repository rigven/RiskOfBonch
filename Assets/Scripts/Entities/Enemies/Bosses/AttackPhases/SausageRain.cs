using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageRain : AttackPhase
{
    private float attackDuration = 7f;
    private float height = 15f;
    private float halfWidth = 5f;
    private Vector2 currentPos;

    new void Start()
    {
        base.Start();
        cooldown = 5f;
        damage *= DifficultyController.difficulty;
    }

    public void Attack()
    {
        if (canStartAttack)
        {
            canStartAttack = false;
            StartCoroutine(StartAttack());
        }

        if (attacking)
        {
            if (Time.realtimeSinceStartup - lastShoot > 0.2f)
            {
                lastShoot = Time.realtimeSinceStartup;
                PushProjectile();
            }
        }
    }


    public IEnumerator StartAttack()
    {
        animator.SetTrigger("Shake");
        currentPos = new Vector2(player.transform.position.x, player.transform.position.y + height);
        attacking = true;
        yield return new WaitForSeconds(attackDuration);
        attacking = false;
        StartCoroutine(AttackCooldown(cooldown));
    }

    private void PushProjectile()
    {
        float xPos = UnityEngine.Random.Range(currentPos.x - halfWidth, currentPos.x + halfWidth);
        Vector2 position = new Vector2(xPos, currentPos.y);
        VariedBullet bullet = Instantiate(projectile, position, Quaternion.identity).GetComponent<VariedBullet>();
        bullet.gameObject.transform.parent = boss.transform;
        bullet.damage = damage;
        Vector3 velocity = Vector2.down * bullet.projectileSpeed;
        bullet.Launch(velocity);
    }
}
