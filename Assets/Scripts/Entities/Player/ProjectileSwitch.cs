using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileSwitch : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableProjectiles = new List<GameObject>();
    [SerializeField] private List<Sprite> projectileIcons = new List<Sprite>();
    private int currentProjectileIndex = 0;
    private PlayerWeapon playerWeapon;
    private Toolbar toolbar;

    private void Start()
    {
        toolbar = FindObjectOfType<Toolbar>();
        playerWeapon = gameObject.GetComponent<PlayerWeapon>();

        toolbar.ChangeTumblersNumber(availableProjectiles.Count, 0);
        ChangeProjectile();
    }

    public void SwitchToNext()
    {
        if (currentProjectileIndex != availableProjectiles.Count - 1)
        {
            currentProjectileIndex++;
        }
        else
        {
            currentProjectileIndex = 0;
        }
        ChangeProjectile();
    }

    public void SwitchToPrev()
    {
        if (currentProjectileIndex != 0)
        {
            currentProjectileIndex--;
        }
        else
        {
            currentProjectileIndex = availableProjectiles.Count - 1;
        }
        ChangeProjectile();
    }

    private void ChangeProjectile()
    {
        playerWeapon.SetProjectile(availableProjectiles[currentProjectileIndex]);
        toolbar.SetIcon(projectileIcons[currentProjectileIndex]);
        toolbar.ChangeActiveTumbler(currentProjectileIndex);
    }
}
