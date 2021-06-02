using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageTentacle : MonoBehaviour
{
    public GameObject target;
    [SerializeField] public GameObject startTarget;
    public Rigidbody2D headTentacleRigidbody;
    public DistanceJoint2D targetJoint;
    public float targetDist = 1;
    float currentDist = 0;
    float closeDist = 3f;
    public float speedX = 100f;
    private float speed;
    public float speedRotation = 1f;
    private float maxDist = 20f;

    void Start()
    {
        startTarget.transform.position = new Vector2(Random.Range(-50, 50), Random.Range(0, 50)).normalized*20;
        target = startTarget;
        StartCoroutine(WaitForPlayer());
    }

    public IEnumerator WaitForPlayer() 
    {
        yield return new WaitForSeconds(2f);
        SetTarget(FindObjectOfType<Player>().gameObject);
    }

    void FixedUpdate()
    {
        currentDist = Vector2.Distance(gameObject.transform.position, target.transform.position);

            if (currentDist > targetDist)
            {
                Vector3 enemyrotation = (target.transform.position - gameObject.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(Vector3.forward, enemyrotation),
                                                      speedRotation * Time.deltaTime);
                speed = speedX * 3;
                if (Vector2.Distance(gameObject.transform.parent.position, transform.position + transform.up * speed * Time.deltaTime) < maxDist)
                {
                    if (currentDist < closeDist)
                    {
                        speed = speedX * 2;
                    }
                    else
                    {
                        speed = speedX;
                    }
                    transform.position += transform.up * speed * Time.deltaTime;
                }

                transform.rotation = Quaternion.LookRotation(Vector3.forward,
                                                        enemyrotation);

            }
            else if (!target.Equals(startTarget))
            {
                targetJoint.connectedBody = headTentacleRigidbody;
            }
    }

    public void SetTarget(GameObject targ)
    {
        target = targ;
        targetJoint = target.GetComponent<DistanceJoint2D>();
        headTentacleRigidbody = GetComponent<Rigidbody2D>();
    }
}
