using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariedBullet : Bullet
{
    [SerializeField] private Sprite[] sprites;

    new void Start()
    {
        lifetime *= 2;
        base.Start();
        GetComponent<SpriteRenderer>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)), transform.localScale.z);
    }

    public void GrowAndLaunch(Vector3 velocity)
    {
        isLaunched = false;
        transform.right = velocity.normalized;
        bufferedVelocity = velocity;
        GetComponent<Animator>().SetBool("Grow", true);
    }

    new public void Launch()
    {
        GetComponent<Animator>().SetBool("Grow", false);
        base.Launch();
    }

    new public void Launch(Vector3 velocity)
    {
        bufferedVelocity = velocity;
        base.Launch();
    }


}
