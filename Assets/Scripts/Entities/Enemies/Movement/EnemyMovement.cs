using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    protected Enemy enemy;
    protected EnemyAttack enemyAttack;
    protected Bounds mapBounds;
    private EnemySpawner enemySpawner;

    [SerializeField] protected float agroRadius = 10f;
    [SerializeField] protected float speed = 4f;
    protected float raycastLenght = Vector2.right.magnitude;

    public bool pursuit = false;
    public bool wanders = false;
    public bool canMove = true;
    public bool canAttack = true;

    protected bool isFlyingType = false;
    protected bool isBoss = false;


    protected void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyAttack = GetComponent<EnemyAttack>();
        mapBounds = FindObjectOfType<CompositeCollider2D>().bounds;
        enemySpawner = FindObjectOfType<EnemySpawner>();
        isBoss = GetComponent<Boss>();


        enemy.targetPlayer.PlayerDied += StopPursuit;
    }

    private void StopPursuit()
    {
        pursuit = false;
    }

    protected void Update()
    {
        if (!GamePause.isPaused)
        {
            enemy.isGrounded = enemy.groundCollider.IsTouchingLayers(enemy.groundMaskNum);
            LookForward();
        }
    }

    private void LookForward()
    {
        if (enemy.targetPlayer)
        {
            bool seesTarget = (Physics2D.Raycast(transform.position, enemy.targetPlayer.transform.position - transform.position, Vector2.Distance(enemy.targetPlayer.transform.position, transform.position), enemy.groundMaskNum).collider == null);

            float distanceToPlayer = Vector2.Distance(transform.position, enemy.targetPlayer.transform.position);

            if (distanceToPlayer >= enemy.distanceToDestroy)
            {
                Destroy(gameObject);
                enemySpawner.mobCount--;
            }

            if (canAttack)
            {
                if (distanceToPlayer <= enemyAttack.attackDistance && seesTarget /*&& pursuit*/)
                {
                    enemyAttack.Attack();
                    if (isFlyingType)
                    {
                        enemy.rigidbody.velocity = new Vector2(0f, 0f);
                    }
                    else
                    {
                        enemy.rigidbody.velocity = new Vector2(0f, enemy.rigidbody.velocity.y);
                    }
                }
            }

            if (enemy.targetPlayer.isInvisible)
            {
                pursuit = false;
                //if (isBoss)
                //{
                //    canMove = false;
                //}
            }
            else if (distanceToPlayer <= agroRadius)
            {
                pursuit = true;
                wanders = false;
                //canMove = true;
            }

            //if (!seesTarget && pursuit)
            //{
            //    Debug.DrawRay(transform.position, enemy.targetPlayer.transform.position - transform.position, Color.yellow);
            //}
            if (canMove && (isBoss &&!enemy.targetPlayer.isInvisible || !isBoss))
            {
                if (pursuit)
                {
                    ReflectToPlayer();

                    if (!seesTarget || Vector2.Distance(transform.position, enemy.targetPlayer.transform.position) > enemyAttack.safeAttackDistance || (!enemy.isGrounded && !isFlyingType))
                    {
                        MoveToPlayer();
                    }
                }
                else
                {
                    Wander();
                }
            }
        }
    }

    virtual protected void ReflectToPlayer()
    {
        transform.localScale = new Vector3(Mathf.Sign(enemy.targetPlayer.transform.position.x - transform.position.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    abstract protected void MoveToPlayer();
    abstract protected void Wander();
}
