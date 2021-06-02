using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    /*[SerializeField]*/ GameObject projectile;
    public Effects effects;        //присваивается в Effects
    Projectile bullet;
    [SerializeField] float attackSpeed;
    private float lastShoot;
    private bool isContinuousShooting;
    private bool isBullet = false;
    private Transform gunParentTrans;
    private Transform barrelParentTrans;
    private Transform curParentTrans;

    private void Start()
    {
        gunParentTrans = transform;
        barrelParentTrans = transform.GetChild(0);
    }

    public void SetProjectile(GameObject projectile)
    {
        if (isContinuousShooting)
        {
            this.projectile.gameObject.GetComponent<ParticleSystem>().Stop();
            Destroy(this.projectile.gameObject);
        }

        if (projectile.gameObject.GetComponent<ParticleSystem>() != null)
        {
            this.projectile = Instantiate(projectile, transform);
            isContinuousShooting = true;
        }
        else
        {
            this.projectile = projectile;
            if (projectile.gameObject.GetComponent<Bullet>())
            {
                curParentTrans = gunParentTrans;
            }
            else
            {
                curParentTrans = barrelParentTrans;
            }
            isContinuousShooting = false;
        }
    }

    void Update()
    {
        Debug.DrawRay(transform.position, (barrelParentTrans.position - transform.position)*10, new Color(0.5f,0.5f,0.5f));
    }

    public void Shoot(bool isShootNeeded)
    {
        if (isContinuousShooting)
        {
            if (isShootNeeded)
            {
                if(projectile.gameObject.GetComponent<ParticleSystem>().isStopped)
                {
                    projectile.gameObject.GetComponent<ParticleSystem>().Play();
                }
            }
            else
            {
                if (projectile.gameObject.GetComponent<ParticleSystem>().isPlaying)
                {
                    projectile.gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
        else
        {
            if (isShootNeeded)
            {
                if (Time.realtimeSinceStartup - lastShoot > projectile.GetComponent<Projectile>().timeBetweenShots)
                {
                    StartCoroutine(PushBullet());
                    lastShoot = Time.realtimeSinceStartup;
                }
            }
        }
    }

    IEnumerator PushBullet()
    {
        yield return new WaitForSeconds(Random.Range(0, 0 + 0.2f));

        if (projectile.GetComponent<Projectile>())  // На случай, если игрок переключился на другой тип снарядов, а корутина уже запущена
        {
            bullet = Instantiate(projectile, curParentTrans.position, curParentTrans.rotation).GetComponent<Projectile>();
            bullet.effects = effects;
            bullet.transform.parent = curParentTrans;
            Vector3 velocity = (Camera.main.WorldToScreenPoint(barrelParentTrans.position) -
                Camera.main.WorldToScreenPoint(gunParentTrans.position)).normalized * bullet.projectileSpeed;
            bullet.Launch(velocity);
        }
    }
}
