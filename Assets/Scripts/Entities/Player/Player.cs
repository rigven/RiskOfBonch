using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField] private int additJumps = 0;       

    private SpriteRenderer animS;
    protected Invisibility invisibility;
    private bool facingRight = true;
    public int jumpCount = 0;

    public bool isInvisible = false;

    GameObject legs;

    public delegate void DeathHandler();
    public event DeathHandler PlayerDied;


    new private void Awake()
    {
        //SetUpSingletone();
        base.Awake();
        legs = transform.Find("Legs").gameObject;

        healthBar = transform.Find("PlayerHPbar").GetComponent<HealthBar>();
        animS = GetComponent<SpriteRenderer>();
    }

    //private void SetUpSingletone()
    //{
    //    if (FindObjectsOfType<Player>().Length > 1)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}
    //private void OnLevelWasLoaded(int level)
    //{
    //    FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
    //    FindObjectOfType<Cinemachine.CinemachineStateDrivenCamera>().m_AnimatedTarget = anim;
    //}


    new private void Update()
    {
        base.Update();
        if (groundCollider.IsTouchingLayers(groundMaskNum)) anim.SetBool("isJumping", false);
        else anim.SetBool("isJumping",true);
    }

    public override void HandleDamage(float damage)
    {
        base.RecieveDamage(damage);
        if (invisibility)
        {
            invisibility.HandleDamage(damage);
        }
    }

    public override void Die()
    {
        isAlive = false;
        GamePause.GameOver();
        PlayerDied?.Invoke();
        Destroy(gameObject);
    }


    public void Move(float move)
    {
        rigidbody.velocity = new Vector2(move * speed, rigidbody.velocity.y);

        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
        {
            Flip();
        }
    }


    public void Jump()
    {
        //groundMaskNum += LayerMask.GetMask("InteractiveObject");
        if (groundCollider.IsTouchingLayers(groundMaskNum)) { jumpCount = 0; }
        if (jumpCount < (additJumps+1))
        {
            jumpCount++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, /*rigidbody.velocity.y + */jumpForce);
        }
    }

    //new public void SetMaxHealth(float mhp)
    //{
    //    base.SetMaxHealth(mhp);
    //}

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = legs.transform.localScale;
        theScale.x *= -1;
        legs.transform.localScale = theScale;
    }

    public void AddInvisibilityModifier(Invisibility invisibility)
    {
        this.invisibility = invisibility;
    }
}