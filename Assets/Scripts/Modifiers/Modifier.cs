using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    MaxHealth,
    Heal,
    ReflectiveShield,
    DamagingShield, 
    Damage,
    Invisibility
}

public class Modifier : MonoBehaviour
{
    SpriteRenderer sp;

    private ModifierType modifierType;
    private float yPos;
    private bool isGrounded = false;
    private bool wasRecieved = false;
    public Dictionary<ModifierType, int> dropList = new Dictionary<ModifierType, int>();
    public Material[] materials;

    // STATIC
    public static Dictionary<ModifierType, string> modifierNames = new Dictionary<ModifierType, string>{
        {ModifierType.ReflectiveShield, "Отражающий щит"},
        {ModifierType.DamagingShield, "Атакующий щит"},
        {ModifierType.Damage, "Увеличение урона"},
        {ModifierType.Invisibility, "Экстренная невидимость"},
    };
    private float pickUpSoundVolume = 2f;
    [SerializeField] public AudioClip pickUpSound;

    public void GenModifierType()
    {
        int modifierNum;

        modifierNum = UnityEngine.Random.Range(0, dropList.Count);

        int maxChance = 0;
        foreach(KeyValuePair<ModifierType, int> chanceValue in dropList) //можно избежать, если макс шанс всегда равен 100
        {
            maxChance += chanceValue.Value;
        }
        //int a = UnityEngine.Random.Range(0, maxChance + 1);
        //Когда генерировался 0, оно не заходило в условие в цикле и тип модификатора оставался первым членом ModifierType.
        int a = UnityEngine.Random.Range(1, maxChance + 1);
        foreach (KeyValuePair<ModifierType, int> chanceValue in dropList)
        {
            maxChance -= chanceValue.Value;
            if (maxChance < a)
            {
                modifierType = chanceValue.Key;
                break;
            }
        }

        
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = Resources.Load<Sprite>("Sprites/Modifiers/Items/mod_" + modifierType.ToString());
        ChangeMaterial();
    }

    private void Update()
    {
        if(isGrounded)
        {
            transform.position = new Vector2(transform.position.x, yPos + Mathf.Sin(Time.timeSinceLevelLoad*2)/3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CompositeCollider2D>())
        {
            isGrounded = true;
            yPos = transform.position.y;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && !wasRecieved)
        {
            AudioSource.PlayClipAtPoint(pickUpSound, FindObjectOfType<Player>().transform.position, pickUpSoundVolume);
            Effects effects = FindObjectOfType<Effects>();
            AddModifier(effects);
            AddEffect(effects);
            wasRecieved = true;
            Destroy(gameObject);
        }
    }

    private void AddModifier(Effects effects)
    {
        if (effects.modifierNumbers.ContainsKey(modifierType))
        {
            if (effects.modifierNumbers[modifierType] == 0)
            {
                effects.modifiersOrder.Add(modifierType);
            }
            effects.modifierNumbers[modifierType]++;
            FindObjectOfType<ModifiersBar>().AddIcon(sp.sprite, modifierType);
        }
    }

    private void AddEffect(Effects effects)
    {
        switch (modifierType)
        {
            case ModifierType.MaxHealth:
                {
                    effects.ModifyMaxHealth(0.1f);
                    break;
                }
            case ModifierType.Heal:
                {
                    effects.HealPercent(0.1f);
                    break;
                }
            case ModifierType.ReflectiveShield:
                {
                    effects.AddReflectiveShield();
                    break;
                }
            case ModifierType.DamagingShield:
                {
                    effects.AddDamagingShield();
                    break;
                }
            case ModifierType.Damage:
                {
                    effects.IncreaseDamage(0.1f);
                    break;
                }
            case ModifierType.Invisibility:
                {
                    effects.AddInvisibility();
                    break;
                }
        }
    }

    private void ChangeMaterial()
    {
        if (materials[(int)modifierType])
        {
            sp.material = materials[(int)modifierType];
        }
    }

    private void ModifyStat(Action<float> Modify, float multiplier)
    {
        Modify(multiplier);
    }
}
