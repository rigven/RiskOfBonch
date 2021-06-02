using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    Rigidbody2D entityRigidbody;
    Animator animator;
    private bool canUse = false;
    [SerializeField] private float jumpForce;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<LivingEntity>() && collision.GetComponent<Rigidbody2D>())
        {
            entityRigidbody = collision.GetComponent<Rigidbody2D>();
            if (Mathf.Abs(entityRigidbody.velocity.y) > 0.1f/*Mathf.Epsilon*/)
            {   
                entityRigidbody.velocity = new Vector2(entityRigidbody.velocity.x, jumpForce);
                animator.SetTrigger("Push");
            }
            if (collision.GetComponent<Player>())
            {
                collision.GetComponent<Player>().jumpCount = 1;
            }
        }
    }
}
