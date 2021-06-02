using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed;
    Material backMaterial;
    Vector2 offset;

    void Start()
    {
        backMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("BackBackground"); ;
        offset = new Vector2(-backgroundScrollSpeed, 0f);
    }

    void Update()
    {
        backMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
