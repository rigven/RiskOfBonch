using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : EnemyAttack
{
    [SerializeField] private GameObject projectile;
    private float attackDelay = 0.05f;
    private Vector2 memorizedPlayerPos;
    private Projectile bullet;

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
            StartCoroutine(PushBullet());
        }
    }

    private IEnumerator PushBullet()
    {
        canAttack = false;
        memorizedPlayerPos = enemy.targetPlayer.transform.position;
        yield return new WaitForSeconds(attackDelay);
        bullet = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        bullet.transform.parent = gameObject.transform;
        bullet.damage = damage;
        Vector2 velocity = (memorizedPlayerPos - (Vector2)transform.position).normalized * bullet.projectileSpeed;
        bullet.Launch(velocity);
        StartCoroutine(AttackCooldown());
    }
}
