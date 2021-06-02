using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProjectileSource
{
    Player,
    Enemy,
    Boss,
    RigidEnemy
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{

    [Header("Specifications")]
    [SerializeField] public float damage = 1;
    [SerializeField] public float projectileSpeed = 1;
    [SerializeField] protected int lifetime = 5;
    [SerializeField] public float timeBetweenShots = 0.1f;
    [Header("Decoration")]
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected float shootSoundVolume = 0.45f;

    protected string projectileParentStr = "Projectiles";
    public ProjectileSource source;
    public Effects effects;    //Присваивается в PlayerWeapon

    protected Rigidbody2D rigidbody;


    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        SetSource();
        if (source == ProjectileSource.Player)
        {
            damage *= effects.damageMultiplier;
        }
        RandomizeVelocity();
        AudioSource.PlayClipAtPoint(shootSound, transform.position, shootSoundVolume);
        ChangeParent();
        Destroy(gameObject, lifetime);
    }

    private void SetSource()
    {
        Enum.TryParse(transform.parent.gameObject.tag, true, out source);
    }

    private void Update()
    {
        
    }

    protected abstract void RandomizeVelocity();

    private void ChangeParent()
    {
        GameObject projectileParent = GameObject.Find(projectileParentStr);
        transform.parent = projectileParent.gameObject.transform;
    }

    public void Launch(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
        TurnTowardsVelocity();
    }

    public void TurnTowardsVelocity()
    {
        transform.right = rigidbody.velocity.normalized;
    }

    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
