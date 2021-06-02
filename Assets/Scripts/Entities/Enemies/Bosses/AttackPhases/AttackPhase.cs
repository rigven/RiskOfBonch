using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPhase : MonoBehaviour
{
    protected float cooldown = 0f;
    protected float damage = 20f;
    [SerializeField] protected VariedBullet projectile;

    protected bool canStartAttack = true;
    protected float lastShoot;
    protected bool attacking = false;
    private bool endPhase = false;

    protected Player player;
    protected Boss boss;
    protected Animator animator;

    protected void Start()
    {
        player = FindObjectOfType<Player>();
        boss = GetComponent<Boss>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator AttackCooldown(float attackCooldown)
    {
        if (!endPhase)
        {
            yield return new WaitForSeconds(attackCooldown);
            canStartAttack = true;
        }
    }

    public IEnumerator PhaseCooldown(float phaseCooldown)
    {
        endPhase = true;
        canStartAttack = false;
        attacking = false;
        yield return new WaitForSeconds(phaseCooldown);
        canStartAttack = true;
        endPhase = false;
    }
}
