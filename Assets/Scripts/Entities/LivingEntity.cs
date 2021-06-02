using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class LivingEntity : MonoBehaviour
{
    [Header("Характеристики")]
    [SerializeField] protected float speed = 4f;
    protected float health;
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] public float jumpForce = 8f;

    [Header("Состояния")]
    public bool isGrounded = false;
    protected bool usesHealthBar = true;
    public bool isAlive {get; protected set; }

    [Header("Константы")]
    public int groundMaskNum;

    [Header("Ссылки на объекты и компоненты")]
    public HealthBar healthBar;
    public Rigidbody2D rigidbody;
    public Collider2D groundCollider;
    protected SpriteRenderer spriteRenderer;
    protected DamageDisplay damageDisplay;
    public Animator anim;

    protected void Awake()
    {
        isAlive = true;
        health = maxHealth;

        damageDisplay = transform.Find("DamageDisplay").GetComponent<DamageDisplay>();
        rigidbody = GetComponent<Rigidbody2D>();
        groundCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        groundMaskNum = LayerMask.GetMask("Ground");
    }

    protected void Update()
    {
        if (!GamePause.isPaused)
        {
            isGrounded = groundCollider.IsTouchingLayers(groundMaskNum);
        }
    }

    public abstract void HandleDamage(float damage);

    public virtual void RecieveDamage(float damage)
    {
        health -= damage;
        healthBar.ChangeHPstatus();
        damageDisplay.ShowDamage(damage, this, true);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    abstract public void Die();

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetHealth(float hp)
    {
        health = Mathf.Clamp(hp, 0, GetMaxHealth());
        healthBar.ChangeHPstatus();
    }

    public void SetMaxHealth(float mhp)
    {
        if (mhp > 0)
        {
            maxHealth = mhp;
            healthBar.ChangeHPstatus();
        }
    }
}
