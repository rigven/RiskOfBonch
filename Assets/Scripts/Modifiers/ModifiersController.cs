using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersController : MonoBehaviour
{
    Player player;
    public float maxHealthPerc;
    public float maxHealth;
    public int invisibility;
    public int damagingShields = 0;
    public int refShields = 0;
    public float damageMultiplier = 1;

    void Start()
    {
        SetUpSingletone();
        player = FindObjectOfType<Player>();
        maxHealth = player.GetMaxHealth();
    }

    private void SetUpSingletone()
    {
        if (FindObjectsOfType<ModifiersController>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        player = FindObjectOfType<Player>();
        player.SetMaxHealth(maxHealth);
    }
}
