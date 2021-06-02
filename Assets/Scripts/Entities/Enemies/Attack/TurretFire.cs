using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFire : Range
{
    SpriteRenderer sp;
    Color idleColor;
    Color attackColor;
    new protected void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        //idleColor = new Color(Color.yellow); 
        //    .SetVector("_EmissionColor", Color.white * 5f);
        //attackColor = sp.material.GetColor("_Color1");
        base.Start();
    }

    new protected void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        if (canAttack)
        {
            enemy.anim.SetTrigger("Fire");

            sp.material.SetVector("_Color1", Color.red * 5f);
        }
        base.Attack();
    }

    public void ChangeAttackLighting()
    {
        sp.material.SetVector("_Color1", Color.yellow * 3f);
    }
}
