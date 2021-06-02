using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : WalkingEnemy
{
    private GameObject barrel;

    new protected void Start()
    {
        base.Start();
        barrel = transform.Find("Barrel").GetChild(0).gameObject;
    }

    new protected void Update()
    {
        base.Update();
        if (!GamePause.isPaused)
        {
            if (enemy.rigidbody.velocity.magnitude > 0f)
            {
                if (!enemy.anim.GetBool("Walk"))
                    enemy.anim.SetBool("Walk", true);
            }
            else
            {
                if (enemy.anim.GetBool("Walk"))
                    enemy.anim.SetBool("Walk", false);
            }
            TurnBarrel();
        }
    }



    private void TurnBarrel()
    {
        if (!wanders)
        {
            Vector3 barrelRotation = (enemy.targetPlayer.transform.position - gameObject.transform.position);

            barrel.transform.right = barrelRotation * enemy.transform.localScale.x;
            //barrel.transform.rotation = Quaternion.LookRotation(Vector3.forward, barrelRotation);
        }
    }
}
