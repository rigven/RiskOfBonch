using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomHealthBar : HealthBar
{
    private TextMeshProUGUI hpText;
    private Vector3 newPos;
    private Vector3 newPosText;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        entity = transform.parent.GetComponent<LivingEntity>();

        slider = Instantiate(sliderPref.transform.GetChild(0).GetComponent<Slider>(), 
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, -1f)),
            Quaternion.identity).GetComponent<Slider>();

        hpText = Instantiate(sliderPref.transform.GetChild(1).GetComponent<TextMeshProUGUI>(),
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, -1f)),
            Quaternion.identity);

        slider.transform.SetParent(canvas.transform.Find("HP bars"), true);
        hpText.transform.SetParent(canvas.transform.Find("HP bars"), true);

        //if (transform.parent.GetComponent<Player>())
        //{
        //    newPos = new Vector3(
        //    (int)(Camera.main.pixelWidth / 6 + Camera.main.pixelWidth / 72),
        //    (int)(Camera.main.pixelHeight - Camera.main.pixelHeight / 16), 0);
        //    newPosText = new Vector3(
        //    (int)(Camera.main.pixelWidth / 6 - 170 / 3 - 8),
        //    (int)(Camera.main.pixelHeight - Camera.main.pixelHeight / 16), 0);
        //}
        //else
        //{
        //       newPos = new Vector3(
        //    (int)(Camera.main.pixelWidth / 3 +  342/ 2),
        //    (int)(Camera.main.pixelHeight - Camera.main.pixelHeight / 16), 0);
        //    newPosText = new Vector3(
        //    (int)(Camera.main.pixelWidth / 3 + 282 / 2/* + 4*/),
        //    (int)(Camera.main.pixelHeight - 42 / 4), 0);
        //}

        //slider.transform.position = newPos;
        //hpText.transform.position = newPosText;

        RectTransform sliderTransform = slider.GetComponent<RectTransform>();
        RectTransform hpTextTransform = hpText.GetComponent<RectTransform>();
        if (transform.parent.GetComponent<Player>())
        {
            sliderTransform.anchorMax = Vector2.up;
            sliderTransform.anchorMin = Vector2.up;
            sliderTransform.pivot = Vector2.up;
            newPos = new Vector2(15f, -9f);
            hpTextTransform.anchorMax = Vector2.up;
            hpTextTransform.anchorMin = Vector2.up;
            hpTextTransform.pivot = Vector2.up;
            newPosText = new Vector2(15f, -9f);
            //newPos = new Vector3(180, 535, 0);
            //newPosText = new Vector3(100, 535, 0);
        }
        else
        {
            sliderTransform.anchorMax = new Vector2(0.5f, 1f);
            sliderTransform.anchorMin = new Vector2(0.5f, 1f);
            sliderTransform.pivot = new Vector2(0.5f, 1f);
            newPos = new Vector2(0, -19f);
            hpTextTransform.anchorMax = new Vector2(0.5f, 1f);
            hpTextTransform.anchorMin = new Vector2(0.5f, 1f);
            hpTextTransform.pivot = new Vector2(0.5f, 1f);
            newPosText = new Vector2(0, -9f);
        }
        sliderTransform.anchoredPosition = newPos;
        hpTextTransform.anchoredPosition = newPosText;

        //hpText.transform.SetParent(canvas.transform.Find("HP bars"), true);
        RedrawHPbar();
    }

    void Update()
    {
        if (percentHP == 0)
        {
            RedrawHPbar();
        }
        if (hadAttacked)
        {
            RedrawHPbar();
        }

        if (slider.transform.localScale.x != 1f)
        {
            slider.transform.localScale = new Vector3(1f, 1f, 1f);
            hpText.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void OnDestroy()
    {
        if (slider)
        {
            Destroy(slider.gameObject);
        }
        if (hpText)
        {
            Destroy(hpText.gameObject);
        }
    }
    private void RedrawHPbar()
    {
        percentHP = entity.GetHealth() / entity.GetMaxHealth();
        slider.value = percentHP;
        hadAttacked = false;

        hpText.text = ((int)Mathf.Ceil(entity.GetHealth())) + "/" + ((int)Mathf.Ceil(entity.GetMaxHealth()));
    }
}
