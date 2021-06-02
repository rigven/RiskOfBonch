using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InteractiveObject : MonoBehaviour
{
    PlayerController playerController;
    protected bool canUse = false;
    protected bool locked = false;
    protected TextMeshProUGUI labelObj;
    protected Vector3 labelTextPosition;
    protected string labelText;
    protected int numberOfUses = 0;


    protected void Start()
    {
        labelObj = FindObjectOfType<Canvas>().transform.Find("Interactive Object Text").gameObject.GetComponent<TextMeshProUGUI>();
        playerController = FindObjectOfType<PlayerController>();
        labelTextPosition = transform.Find("LabelTextPivot").position;
    }

    protected void Update()
    {
        if (canUse && !locked)
        {
            labelObj.transform.position = Camera.main.WorldToScreenPoint(labelTextPosition);       // В Update, так как иначе следует за камерой
            if (playerController.usesObject)
            {
                UseObject();
                playerController.usesObject = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!locked)
        {
            canUse = true;
            labelObj.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canUse = false;
        labelObj.gameObject.SetActive(false);
    }

    protected abstract void UseObject();
}
