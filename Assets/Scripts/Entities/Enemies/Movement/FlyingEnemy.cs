using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyMovement
{
    private float steeringToPlayerAngle = 0.25f;
    private float wanderRadius = 1f;
    private float wanderAngle = 0f;
    private Vector3 wanderDirection;


    new protected void Start()
    {
        base.Start();
        isFlyingType = true;
    }

    new protected void Update()
    {
        base.Update();
    }

    protected override void MoveToPlayer()
    {
        Vector2 direction = (enemy.targetPlayer.transform.position - gameObject.transform.position).normalized;

        enemy.rigidbody.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    protected override void Wander()
    {
        if (wanders)
        {
            wanderAngle += Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)) * 5;
            CalculateWanderDirection();

            if (!mapBounds.Contains(wanderDirection))
            {
                wanderAngle -= 180;
                CalculateWanderDirection();
            }
            transform.rotation = Quaternion.identity;
            enemy.rigidbody.velocity = (wanderDirection - transform.position).normalized * speed;
            Debug.DrawRay(transform.position, enemy.rigidbody.velocity, Color.blue);
            transform.localScale = new Vector3(Mathf.Sign(enemy.rigidbody.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            wanders = true;
            wanderAngle = UnityEngine.Random.Range(0, 360);
        }
    }

    private void CalculateWanderDirection()
    {
        SteerToPlayerSide();
        wanderDirection = new Vector3(transform.position.x + wanderRadius * Mathf.Cos(wanderAngle * Mathf.Deg2Rad), transform.position.y + wanderRadius * Mathf.Sin(wanderAngle * Mathf.Deg2Rad), transform.position.z);
    }

    private void SteerToPlayerSide()
    {
        Vector2 wanderDirection = new Vector3(wanderRadius * Mathf.Cos(wanderAngle * Mathf.Deg2Rad), wanderRadius * Mathf.Sin(wanderAngle * Mathf.Deg2Rad), 0);
        Vector2 vectorToPlayer = enemy.targetPlayer.transform.position - gameObject.transform.position;
        float wanderToPlayerAngle = Vector2.SignedAngle(wanderDirection, vectorToPlayer);
        wanderAngle = wanderToPlayerAngle < 0 ? wanderAngle - steeringToPlayerAngle : wanderAngle + steeringToPlayerAngle;
    }
}
