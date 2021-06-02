using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopirkinMovenent : FlyingEnemy
{
    new protected void Start()
    {
        base.Start();
    }

    new protected void Update()
    {
        base.Update();

        if (pursuit)
        {
            enemy.anim.SetBool("Pursuit", true);
        }
        else
        {
            enemy.anim.SetBool("Pursuit", false);
        }
    }

}
