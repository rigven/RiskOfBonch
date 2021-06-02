using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    GameObject background;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newCamPos = new Vector2(transform.position.x, transform.position.y);
        background.transform.position = new Vector3((float)Math.Round(newCamPos.x, 3), (float)Math.Round(newCamPos.y, 3), background.transform.position.z);
        //transform.position = new Vector3(newCamPos.x, newCamPos.y, transform.position.z);
    }
}
