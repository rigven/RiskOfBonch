using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModifierIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ModifierType type;
    private ModifiersBar modifiersBar;

    private void Start()
    {
        modifiersBar = transform.parent.GetComponent<ModifiersBar>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        modifiersBar.label.transform.position = new Vector3(modifiersBar.label.transform.position.x, transform.position.y, 0);
        modifiersBar.label.gameObject.SetActive(true);
        modifiersBar.label.text = Modifier.modifierNames[type];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        modifiersBar.label.gameObject.SetActive(false);
    }
}
