using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private float phaseCooldown = 3;
    protected short phaseNumber = 3;
    public short currentPhase = 1;
    public bool arrived = false;    // Для сосичного босса вместо нее можно испоьзовать canStartAttack, но для движущихся боссов понадобится отдельная переменная
    private float arrivingTime = 3f;

    protected AttackPhase attackPhase;
    protected Animator animator;

    new protected void Awake()
    {
        maxHealth /= DifficultyController.difficulty;
        maxHealth *= 1 + (DifficultyController.difficulty-1)/2;
        DifficultyController.isBossFight = true;
        attackPhase = GetComponent<AttackPhase>();
        targetPlayer = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        base.Awake();
    }

    protected void Start()
    {
        print(enemyMovement);
        if (enemyMovement)
        {
            enemyMovement.canMove = false;
            enemyMovement.canAttack = false;
        }
        StartCoroutine(Arrive());
    }

    private IEnumerator Arrive()
    {
        yield return new WaitForSeconds(arrivingTime);
        if (FindObjectOfType<DifficultyController>())
            DifficultyController.spawnIntensity = 2f;
        arrived = true;
        if (enemyMovement)
        {
            enemyMovement.canMove = true;
            enemyMovement.canAttack = true;
        }
    }

    new protected void Update()
    {
        base.Update();
    }

    public override void HandleDamage(float damage)
    {
        if (arrived)
        {
            base.RecieveDamage(damage);

            UpdatePhase();
        }
    }

    private void UpdatePhase()
    {
        if (health / maxHealth <= 1 - 1f / phaseNumber * currentPhase)
        {
            //StartCoroutine(attackPhase.PhaseCooldown(phaseCooldown));
            currentPhase++;
        }
    }

    public override void Die()
    {
        DifficultyController.isBossFight = false;
        Destroy(gameObject);
        ScoreController.score += killPoints;
        transform.parent.gameObject.GetComponent<Portal>().SwitchToNextLevel();

        if (deathVFX)
        {
            ParticleSystem dhvfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
            //dhvfx.startSize *= Mathf.Abs(transform.localScale.x);
            if (GetComponent<RotatingFlyingEnemy>())
            {
                dhvfx.startRotation = transform.localEulerAngles.z / 55.55f;   // Работает, по возможности сделать нормально
                dhvfx.transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            else
            {
                dhvfx.transform.localScale = transform.localScale;
            }
        }
    }
}
