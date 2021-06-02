using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherGrenade : Projectile
{
    private float explosionRadius = 0.75f;

    [SerializeField] protected AudioClip explosionSound;
    [SerializeField] protected ParticleSystem explosionEffectPrefab;
    protected float explosionSoundVolume = 0.60f;

    new void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        string[] collisionLayers = new string[] { "Enemy", "FlyingEnemy", "RigidEnemy" };
        int mask = LayerMask.GetMask(collisionLayers);
        Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, mask);
        foreach (Collider2D collider in affectedColliders)
        {
            if (!collider.isTrigger && collider.gameObject.GetComponent<LivingEntity>())
            {
                collider.gameObject.GetComponent<LivingEntity>().HandleDamage(damage);
            }
        }
        DestroyProjectile();
    }

    protected override void RandomizeVelocity()
    {
        rigidbody.velocity *= Random.Range(0.95f, 1.05f);
    }
}
