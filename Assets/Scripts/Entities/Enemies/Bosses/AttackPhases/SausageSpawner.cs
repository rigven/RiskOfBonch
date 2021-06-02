using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageSpawner : MonoBehaviour
{
    [SerializeField] GameObject sausageTentaclePref;
    private GameObject tail;
    private CompositeEnemy head;
    private float lastMass = 0;
    private float dist = 1f;
    private HingeJoint2D joint;
    private Rigidbody2D rb2d;
    private int sausCount;

    void Start()
    {
        sausCount = 1;
        tail = Instantiate(sausageTentaclePref, transform);
        tail.transform.GetChild(0).GetComponent<CompositeEnemy>().isHead = true;
        head = tail.transform.GetChild(0).GetComponent<CompositeEnemy>();

        rb2d = tail.GetComponent<Rigidbody2D>();
        lastMass = rb2d.mass;
        rb2d.mass = 60;

        joint = tail.GetComponent<HingeJoint2D>();
        joint.connectedAnchor = new Vector2(0, 0);
        joint.connectedBody = gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (sausCount < 22)
        {
            if (Vector2.Distance(gameObject.transform.position, (Vector2)tail.transform.position + joint.anchor) > dist) 
            {
                sausCount++;
                CreatePart();
            }
        }
        else
        {
            tail.transform.position = transform.position;
        }
    }

    private void CreatePart()
    {
        tail = Instantiate(sausageTentaclePref, transform);
        CompositeEnemy ce = tail.transform.GetChild(0).GetComponent<CompositeEnemy>();
        head.parts.Add(ce);
        tail.transform.GetChild(0).GetComponent<CompositeEnemy>().mainPart = gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<CompositeEnemy>();

        joint = tail.GetComponent<HingeJoint2D>();
        joint.connectedBody = rb2d;

        lastMass = rb2d.mass * 1.05f;
        rb2d = tail.GetComponent<Rigidbody2D>();
        rb2d.mass = Mathf.Clamp(lastMass, 10, 20);
    }
}
