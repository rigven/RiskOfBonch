using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public abstract class WalkingEnemy : EnemyMovement
{
    private float wanderDirection = 0f;
    private float wanderTime = 0f;
    private Vector2 wanderNextStep;
    SpriteRenderer spriteRenderer;

    new protected void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFlyingType = false;
    }

    new protected void Update()
    {
        base.Update(); 
        if (!GamePause.isPaused)
        {
            Jump();
            if (!enemy.isGrounded)
            {
                enemy.anim.SetBool("Jump", true);
            }
            else
            {
                enemy.anim.SetBool("Jump", false);
            }
        }
    }

    private void Jump()
    {
        if (enemy.rigidbody.velocity.x != 0)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position - new Vector3(0, 0.5f, 0) * transform.localScale.y, Vector2.right * transform.localScale.x, raycastLenght, enemy.groundMaskNum);
            Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0) * transform.localScale.y, Vector2.right * Mathf.Sign(transform.localScale.x) * raycastLenght, Color.white);

            if (enemy.isGrounded)
            {
                if (raycastHit.collider != null)
                {
                    Debug.DrawRay(transform.position  - new Vector3(0, 0.5f, 0) * transform.localScale.y, Vector2.right * transform.localScale.x * raycastLenght, Color.yellow);
                    enemy.rigidbody.velocity = new Vector2(enemy.rigidbody.velocity.x, enemy.jumpForce);
                }
            }
        }
    }

    protected override void MoveToPlayer()
    {
        Vector2 direction = (enemy.targetPlayer.transform.position - gameObject.transform.position).normalized;
        enemy.rigidbody.velocity = new Vector2(direction.x * speed, enemy.rigidbody.velocity.y);
    }

    protected override void Wander()
    {
        wanderNextStep = new Vector2(transform.position.x + wanderDirection, transform.position.y);

        if (!mapBounds.Contains(wanderNextStep))
        {
            wanderDirection *= -1f;
        }

        if (!wanders)
        {
            wanders = true;
            wanderDirection = Mathf.Sign(UnityEngine.Random.Range(-1f, 1f));
            wanderTime = UnityEngine.Random.Range(0f, 3f);
            StartCoroutine(ResetWander(wanderTime));
        }
        else
        {
            transform.localScale = new Vector3(wanderDirection, transform.localScale.y, transform.localScale.z);
            enemy.rigidbody.velocity = new Vector2(wanderDirection * speed, enemy.rigidbody.velocity.y);
        }
    }

    private IEnumerator ResetWander(float time)
    {
        yield return new WaitForSeconds(time);
        wanders = false;
    }
}
