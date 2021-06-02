using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InscriptionPos : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    public bool isScore = false;
    float offsetX = 0.3f;
    float offsetY = 1.7f;

    void Start()
    {
        if (!isScore) { offsetX = 2.8f; }
        transform.position = Camera.main.ScreenToWorldPoint(targetObject.transform.position);
        transform.position = new Vector3(transform.position.x - offsetX, transform.position.y - offsetY, 0);
    }
}
