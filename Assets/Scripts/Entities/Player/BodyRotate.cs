using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotate : MonoBehaviour
{
    [SerializeField] public int speedRotation = 200;
    private Player player;
    // Start is called before the first frame update
    void Start() 
    {
        player = transform.parent.GetComponent<Player>();
    }
    void Update()
    {
        if (!GamePause.isPaused)
        {
            RotateGunsight();
        }
    }

    private void RotateGunsight()
    {
        Vector3 mouseRotation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z)
                    - Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.5f, 0));
        if (transform.localScale.x != Mathf.Sign(mouseRotation.x))
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            mouseRotation.x *= -1;
        }

        if (Mathf.Abs(Mathf.Sin(Vector2.Angle(Vector2.up, mouseRotation) * Mathf.Deg2Rad)) > 1f / 2f || mouseRotation.y > 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseRotation);
            transform.rotation = transform.rotation * Quaternion.AngleAxis(90f * transform.localScale.x, Vector3.forward);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(-60f * transform.localScale.x, Vector3.forward);
        }
    }
}
