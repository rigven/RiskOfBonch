using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingFire : MonoBehaviour
{
    Effects effects;
    PlayerController playerController;
    Collider2D collider;
    LayerMask rigidbodiesMask;
    int enemiesMask;
    [SerializeField] float damage = 0.1f;
    private float currendDamage;
    [SerializeField] float cooldown = 0.03f;
    [SerializeField] int maxCollisionsNumber = 15;
    private float lastShoot;

    void Start()
    {
        rigidbodiesMask = LayerMask.GetMask(new string[] { "Enemy", "Ground", "FlyingEnemy", "RigidEnemy" });
        enemiesMask = LayerMask.GetMask(new string[] { "Enemy", "FlyingEnemy", "RigidEnemy" });
        effects = FindObjectOfType<Effects>();
        playerController = FindObjectOfType<PlayerController>();

        var collision = GetComponent<ParticleSystem>().collision;
        collision.collidesWith = rigidbodiesMask;
        collider = transform.GetChild(0).GetComponent<Collider2D>();

        currendDamage = damage;
    }

    private void Update()
    {
        currendDamage = damage * effects.damageMultiplier;

        if (playerController.fire && Time.realtimeSinceStartup - lastShoot > cooldown)
        {
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(enemiesMask);
            Collider2D[] affectedColliders = new Collider2D[maxCollisionsNumber];
            Physics2D.OverlapCollider(collider, contactFilter, affectedColliders);

            foreach (Collider2D collider in affectedColliders)
            {
                if (collider && collider.gameObject.GetComponent<LivingEntity>())
                {
                    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, collider.gameObject.transform.position - transform.position, 20f, rigidbodiesMask);

                    if (raycastHit && raycastHit.collider.gameObject.GetComponent<LivingEntity>())
                    {
                        collider.gameObject.GetComponent<LivingEntity>().HandleDamage(currendDamage);
                    }
                }
            }
        }
    }
}
