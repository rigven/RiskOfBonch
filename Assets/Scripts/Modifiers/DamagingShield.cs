using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingShield : MonoBehaviour
{
    private GameObject center;
    public Effects effects;   //ставится в Effects при создании
    public float damage;
    public float defaultDamage = 30f;
    public int angle = 0; 
    float x = 0;
    float y = 0;
    private float cooldown = 8f;
    SpriteRenderer spriteRenderer;
    Color inactiveColor;
    Color activeColor;
    private bool isActive = true;
    public int shieldNum = 1;
    [SerializeField] Sprite activeSprite; 
    [SerializeField] Sprite inactiveSprite; 

    private void Start()
    {
        center = transform.parent.gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //effects = FindObjectOfType<Effects>();
        inactiveColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        activeColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        damage = defaultDamage * effects.damageMultiplier;
    }

    void Update()
    {
        Relocate();
    }

    private void Relocate()
    {
        angle++;
        if (angle > 360)
        {
            angle = 0;
        }
        x = center.transform.position.x + 2 * Mathf.Cos((angle + (360 * shieldNum / effects.damagingShields.Count)) * Mathf.Deg2Rad);
        y = center.transform.position.y + 2 * Mathf.Sin((angle + (360 * shieldNum / effects.damagingShields.Count)) * Mathf.Deg2Rad);
        transform.position = new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            if (collision.gameObject.GetComponent<CompositeEnemy>())
            {
                collision.gameObject.GetComponent<CompositeEnemy>().HandleDamage(damage);
                StartCoroutine(deactivateShield());
            }
            else if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().HandleDamage(damage);
                StartCoroutine(deactivateShield());
            }
        }
    }

    private IEnumerator deactivateShield()
    {
        isActive = false;
        spriteRenderer.color = inactiveColor;
        spriteRenderer.sprite = inactiveSprite;
        yield return new WaitForSeconds(cooldown);
        isActive = true;
        spriteRenderer.sprite = activeSprite;
        spriteRenderer.color = activeColor;
    }
}
