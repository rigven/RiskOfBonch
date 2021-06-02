using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeEnemy : Enemy
{
    public CompositeEnemy mainPart;
    public List<CompositeEnemy> parts;
    public bool isHead = false;

    new private void Awake()
    {
        maxHealth /= DifficultyController.difficulty;
        maxHealth *= 1 + (DifficultyController.difficulty - 1) / 2;
        usesHealthBar = false;

        base.Awake();
    }

    new private void Update()
    {
        base.Update();
    }

    public override void HandleDamage(float damage)
    {
        damageDisplay.ShowDamage(damage, this, false);
        if (!isHead)
        {
            mainPart.HandleDamage(damage);
        }
        else
        {
            health -= damage;
            if (health <= 0 && isAlive)
            {
                health = 0;
                Die();
            }
        }
    }

    public override void Die()
    {
        isAlive = false;
        if (isHead)
            if (deathVFX)
            {
                int count = 0;
                foreach(CompositeEnemy part in parts)
                {
                    count++;
                    ParticleSystem dhvfx = Instantiate(deathVFX, part.transform.position, Quaternion.identity);
                    Destroy(dhvfx, 2f);
                }
            }
        AudioSource.PlayClipAtPoint(deathSound, FindObjectOfType<Player>().transform.position, deathSoundVolume);
        Destroy(gameObject.transform.parent.parent.gameObject);
    }
}
