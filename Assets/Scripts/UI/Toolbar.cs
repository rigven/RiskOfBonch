using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour
{
    [SerializeField] private Image inactiveTumblerPrefab;
    [SerializeField] private Image activeTumblerPrefab;
    [SerializeField] private float distanceBetweenTumblers = 12f;
    [SerializeField] private float localPosY = 28f;
    private List<Image> tumblers = new List<Image>();
    private int currentActiveTumbler;

    private Image imageObj;

    private void Start()
    {
        imageObj = gameObject.transform.Find("Projectile icon").GetComponent<Image>();
    }

    public void ChangeTumblersNumber(int number, int activeTumblerIndex)
    {
        tumblers = new List<Image>();
        float offset = (number - 1f) / 2f;

        for (int i = 0; i < number; i++)
        {
            Image tumbler;
            float localPosX = (i - offset) * distanceBetweenTumblers;
            if (i == activeTumblerIndex)
            {
                tumbler = Instantiate(activeTumblerPrefab, transform);
                currentActiveTumbler = activeTumblerIndex;
            }
            else
            {
                tumbler = Instantiate(inactiveTumblerPrefab, transform);
            }
            tumbler.rectTransform.localPosition = new Vector3(localPosX, localPosY, tumbler.rectTransform.localPosition.z);
            tumblers.Add(tumbler);
        }
    }

    public void ChangeActiveTumbler(int newActiveTumblerIndex)
    {
        tumblers[currentActiveTumbler].sprite = inactiveTumblerPrefab.sprite;
        tumblers[newActiveTumblerIndex].sprite = activeTumblerPrefab.sprite;
        currentActiveTumbler = newActiveTumblerIndex;
    }

    public void SetIcon(Sprite icon)
    {
        imageObj.sprite = icon;
    }
}
