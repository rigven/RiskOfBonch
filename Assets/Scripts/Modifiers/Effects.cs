using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects instance = null;

    private Player player;
    private Transform modifiersParentTransform;
    public float maxHealth;
    public float damageMultiplier = 1;

    private Invisibility invisibility;
    private GameObject damagingShieldParent;
    private ReflectiveShield reflectiveShield;
    public List<DamagingShield> damagingShields = new List<DamagingShield>();
    public Dictionary<ModifierType, int> modifierNumbers = new Dictionary<ModifierType, int>{
        {ModifierType.ReflectiveShield, 0},
        {ModifierType.DamagingShield, 0},
        {ModifierType.Damage, 0},
        {ModifierType.Invisibility, 0},
    };
    public List<ModifierType> modifiersOrder = new List<ModifierType>();
    private bool willBeDestroyed = false;


    private void Awake()
    {
        //SetUpSingletone();
    }

    private void Start()
    {
        SetUpSingletone();

        if (!willBeDestroyed)
        {
            UpdateRefs();
            if (maxHealth == 0)
            {
                maxHealth = player.GetMaxHealth();
                player.SetHealth(maxHealth);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        SetUpSingletone();

        if (!willBeDestroyed)
        {
            UpdateRefs();

            if (maxHealth == 0)
            {
                maxHealth = player.GetMaxHealth();
            }
            player.SetMaxHealth(maxHealth);
            player.SetHealth(maxHealth);
            SetDamagingShield();
            SetReflectiveShield();
            SetInvisibility();
            SetIcons();
        }
    }

    private void SetUpSingletone()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            willBeDestroyed = true;
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateRefs()
    {
        player = FindObjectOfType<Player>();
        if (player)
        {
            invisibility = player.gameObject.AddComponent<Invisibility>();
            FindObjectOfType<ModifiersBar>().effects = this;
            FindObjectOfType<PlayerWeapon>().effects = this;
            player.PlayerDied += DeleteAllModifiers;
            modifiersParentTransform = Instantiate(new GameObject("Modifiers"), player.transform).transform;
        }
    }

    private void SetIcons()
    {
        foreach (ModifierType modifier in modifiersOrder)
        {
            FindObjectOfType<ModifiersBar>().AddIcon(Resources.Load<Sprite>("Sprites/Modifiers/Items/mod_" + modifier.ToString()), modifier);
        }
    }

    public void ModifyMaxHealth(float percent)
    {
        if (percent > 0)
        {
            int maxHealth = (int)(player.GetMaxHealth() * (percent + 1f));
            player.SetMaxHealth(maxHealth);
            HealPercent(0.1f);
            this.maxHealth = maxHealth;
        }
    }

    public void HealPercent(float percent)
    {
        if (percent > 0)
        {
            int healing = (int)(player.GetMaxHealth() * percent);
            player.SetHealth(player.GetHealth() + healing);
        }
    }

    public void IncreaseDamage(float percent)
    {
        if (percent > 0)
        {
            damageMultiplier *= (1f + percent);
            foreach (DamagingShield damageShield in damagingShields)
            {
                damageShield.damage = damageShield.defaultDamage * damageMultiplier;
            }
        }
    }

    public void AddReflectiveShield()
    {
        if (!reflectiveShield)
        {
            reflectiveShield = Instantiate(Resources.Load<ReflectiveShield>("Prefabs/Modifiers/" + ModifierType.ReflectiveShield.ToString()), modifiersParentTransform);
        }
        else
        {
            reflectiveShield.AddModification();
        }
    }

    private void SetReflectiveShield()
    {
        if (modifierNumbers[ModifierType.ReflectiveShield] > 0)
        {
            reflectiveShield = Instantiate(Resources.Load<ReflectiveShield>("Prefabs/Modifiers/" + ModifierType.ReflectiveShield.ToString()), modifiersParentTransform);
            reflectiveShield.SetStartingModifications(modifierNumbers[ModifierType.ReflectiveShield] - 1);
        }
    }

    public void AddDamagingShield()
    {
        if (!damagingShieldParent)
        {
             damagingShieldParent = Instantiate(new GameObject(ModifierType.DamagingShield.ToString()), modifiersParentTransform);
        }
        damagingShields.Add(Instantiate(Resources.Load<DamagingShield>("Prefabs/Modifiers/" + ModifierType.DamagingShield.ToString()), damagingShieldParent.transform));
        damagingShields[damagingShields.Count - 1].effects = this;
        damagingShields[damagingShields.Count - 1].shieldNum = damagingShields.Count;
        foreach (DamagingShield damageShield in damagingShields) 
        {
            damageShield.angle = 0;
        }
    }

    private void SetDamagingShield()
    {
        if (ModifierType.DamagingShield > 0)
        {
            damagingShields = new List<DamagingShield>();
            for (int i = 0; i < modifierNumbers[ModifierType.DamagingShield]; i++)
            {
                AddDamagingShield();
            }
        }
    }

    public void AddInvisibility()
    {
        if (!invisibility.isEnabled)
        {
            invisibility.isEnabled = true;
        }
        else
        {
            invisibility.AddModification();
        }
    }

    public void SetInvisibility()
    {
        if (modifierNumbers[ModifierType.Invisibility] > 0)
        {
            invisibility.isEnabled = true;
            invisibility.SetStartingModifications(modifierNumbers[ModifierType.Invisibility] - 1);
        }
    }

    private void DeleteAllModifiers()
    {
        Destroy(this);
    }
}
