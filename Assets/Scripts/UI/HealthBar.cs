using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    protected LivingEntity entity;
    protected float percentHP;
    protected bool hadAttacked = false;
    [SerializeField] protected GameObject sliderPref;
    protected Slider slider;
    protected Canvas canvas;

    void Start()
    {
        slider = gameObject.AddComponent<Slider>();
        entity = transform.parent.gameObject.GetComponent<LivingEntity>();
        canvas = FindObjectOfType<Canvas>();

        slider = Instantiate(sliderPref, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, -1f)), Quaternion.identity).GetComponent<Slider>();
        slider.transform.SetParent(canvas.transform.Find("HP bars"), true);

        RedrawHPbar(); 
    }

    void Update()
    {
        slider.transform.position =  Camera.main.WorldToScreenPoint(transform.position);
        if (hadAttacked)
        {
            RedrawHPbar();
        }
    }

    private void OnDestroy()
    {
        if (slider)
        {
            Destroy(slider.gameObject);
        }
    }

    public void ChangeHPstatus()
    {
        hadAttacked = true;
    }

    private void RedrawHPbar()
    {
        percentHP = (float)entity.GetHealth() / entity.GetMaxHealth();
        slider.value = 1f - percentHP;
        hadAttacked = false;
    }
}
