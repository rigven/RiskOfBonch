using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLooping : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 1f;
    Material backMaterial;
    Vector2 offset;
    Vector3 cameraPrevPos;


    void Start()
    {
        backMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("BackBackground");
        cameraPrevPos = Camera.main.transform.position;
    }


    void Update()
    {
        offset = Camera.main.transform.position - cameraPrevPos;
        backMaterial.mainTextureOffset += offset * 0.01f /** Time.deltaTime */ * speedMultiplier;
        cameraPrevPos = Camera.main.transform.position;
    }
}
