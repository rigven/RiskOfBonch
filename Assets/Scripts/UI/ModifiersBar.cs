using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifiersBar : MonoBehaviour
{
    [SerializeField] Image iconPrefab;
    public Effects effects;   // ставится в Effects
    public TextMeshProUGUI label;
    private Dictionary<ModifierType, TextMeshProUGUI> modifierNumberTexts = new Dictionary<ModifierType, TextMeshProUGUI>();

    private void Start()
    {
        label = transform.Find("Label").GetComponent<TextMeshProUGUI>();
    }

    public void AddIcon(Sprite sprite, ModifierType modifierType)
    {
        if (!modifierNumberTexts.ContainsKey(modifierType))
        {
            Image icon = Instantiate(iconPrefab, transform.position + new Vector3(0, iconPrefab.rectTransform.sizeDelta.y * modifierNumberTexts.Count, 0), Quaternion.identity);
            icon.rectTransform.SetParent(transform, true);
            icon.sprite = sprite;
            icon.gameObject.GetComponent<ModifierIcon>().type = modifierType;
            modifierNumberTexts.Add(modifierType, icon.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
        modifierNumberTexts[modifierType].text = "x" + effects.modifierNumbers[modifierType];
    }
}
