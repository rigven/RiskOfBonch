using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartureToSide : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    Vector2 dir;

    void Start()
    {
        float xDir = UnityEngine.Random.Range(-1f, 1f);
        float yDir = UnityEngine.Random.Range(-1f, 1f);
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * Mathf.Sign(xDir), transform.localScale.y);
        dir = new Vector2(xDir, yDir).normalized;
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + dir.x * speed * Time.deltaTime, transform.position.y + dir.y * speed * Time.deltaTime);
    }
}
