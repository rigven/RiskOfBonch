using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPursuer : MonoBehaviour
{
    private void Update()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }
}
