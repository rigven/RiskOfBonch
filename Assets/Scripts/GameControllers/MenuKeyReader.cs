using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKeyReader : MonoBehaviour
{
    [SerializeField] SpriteRenderer logo;
    [SerializeField] SpriteRenderer enterText;
    [SerializeField] SpriteRenderer buildings;
    private float yPos;
    private float xPos;
    float i = 0;
    private void Start()
    {
        yPos = enterText.gameObject.transform.position.y;
        xPos = enterText.gameObject.transform.position.x;
    }
    private IEnumerator LogoMovement()
    {
        yield return new WaitForSeconds(1f);

    }
    void Update()
    {
        if (yPos < 1f)
        {
            yPos = yPos + 1f * Time.deltaTime;
            enterText.gameObject.transform.position =
                new Vector2(xPos, yPos);
            logo.gameObject.transform.position =
                new Vector2(xPos, logo.gameObject.transform.position.y - Time.deltaTime);
        }
        else
        {
            i += 0.01f;
            if (i >= 12.5f) i = 0;
            enterText.gameObject.transform.position = new Vector2(xPos, yPos + Mathf.Sin(i * 1 / 2) / 4);
        }

        if (buildings.gameObject.transform.position.y < 1.8f)
            buildings.gameObject.transform.position =
                new Vector2(buildings.gameObject.transform.position.x, buildings.gameObject.transform.position.y + Time.deltaTime);
        
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.JoystickButton10))
        {
            FindObjectOfType<SceneLoader>().LoadFirstLevel();
        }
    }

}
