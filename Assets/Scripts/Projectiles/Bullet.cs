using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    int? mask = null;
    bool canBeReflected = true;
    protected bool isLaunched = true;

    protected Vector3 bufferedVelocity;

    new void Start()
    {
        base.Start();
        ChooseCollisionLayers();
    }

    void Update()
    {
        if (!GamePause.isPaused)
        {
            CheckCollisions();
        }
    }

    private void CheckCollisions()
    {
        ChooseCollisionLayers();
        if (mask != null && isLaunched)
        {
            float raycastLenght = rigidbody.velocity.magnitude;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, rigidbody.velocity, raycastLenght * Time.deltaTime, (int)mask);
            if (raycastHit.collider != null)
            {
                HandleCollision(raycastHit.collider.gameObject);
            }
            else
            {
                Debug.DrawRay(transform.position, rigidbody.velocity.normalized * raycastLenght * Time.deltaTime, Color.white);
            }
        }
    }

    public void ChooseCollisionLayers()
    {
        string[] collisionLayers;
        switch (source)
        {
            case ProjectileSource.Player:
                collisionLayers = new string[] { "Enemy", "Ground", "FlyingEnemy", "RigidEnemy" };
                break;
            case ProjectileSource.Enemy:
                collisionLayers = new string[] { "Player", "Ground", "Shield" };
                break;
            case ProjectileSource.RigidEnemy:
                collisionLayers = new string[] { "Player", "Ground", "Shield" };
                break;
            case ProjectileSource.Boss:
                collisionLayers = new string[] { "Player", "Shield" };
                break;
            default:
                collisionLayers = new string[] { "Enemy", "Ground", "Player", "FlyingEnemy", "Shield", "RigidEnemy" };
                break;
        }
        mask = LayerMask.GetMask(collisionLayers);
    }

    private void HandleCollision(GameObject collision)
    {
        if (collision.GetComponent<ReflectiveShield>())
        {
            if (canBeReflected)
            {
                collision.GetComponent<ReflectiveShield>().HandleCollision(this);
                canBeReflected = false;
            }
            return;
        }
        else if (collision.GetComponent<LivingEntity>())
        {
            collision.GetComponent<LivingEntity>().HandleDamage(damage);
        }
        else if (collision.transform.parent.GetComponent<LivingEntity>())
        {
            collision.transform.parent.GetComponent<LivingEntity>().HandleDamage(damage);
        }
        DestroyProjectile();
    }

    protected override void RandomizeVelocity()
    {
        rigidbody.velocity *= Random.Range(1, 1.05f);
    }

    public void Launch()
    {
        isLaunched = true;
        base.Launch(bufferedVelocity);
        ChooseCollisionLayers();
    }
}
