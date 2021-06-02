using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersSpawner : MonoBehaviour
{
    private Dictionary<ModifierType, int> dropDict = new Dictionary<ModifierType, int>();
    [SerializeField] public List<ModifierType> dropList = new List<ModifierType>();
    [SerializeField] public List<int> dropChances = new List<int>();

    [SerializeField] protected Modifier modifierPref;

    void Start()
    {
        SetLoot();
        for (int i = 0; i < transform.childCount; i++)
        {
            SpawnModifier(transform.GetChild(i).gameObject);
        }
    }


    public void SetLoot()
    {
        for (int i = 0; i < dropChances.Count; i++)
        {
                dropDict.Add(dropList[i], dropChances[i]);
        }
    }
    public void SpawnModifier(GameObject modPoint)
    {
        if (dropDict.Count != 0)
        {
            Modifier modifier = Instantiate(
                modifierPref,
                new Vector3(modPoint.transform.position.x, modPoint.transform.position.y + 0.5f, modPoint.transform.position.z),
                Quaternion.identity);
            modifier.dropList = dropDict;
            modifier.GenModifierType();
        }
    }
}
