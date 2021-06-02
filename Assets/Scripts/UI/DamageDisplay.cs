using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab;
    [SerializeField] float maxDistanceFromPivot = 2f;
    private Canvas canvas;
    public List<DamageText> texts = new List<DamageText>();
    private static int maxNumberOnChar = 10;
    private Transform parentObjectTransform;

    public void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        parentObjectTransform = canvas.transform.Find("HP bars");
    }

    public void Update()
    {
        DeleteOld();
        MoveNumbersAfterCharacter();
    }

    private void DeleteOld()
    {
        if (texts.Count >= maxNumberOnChar)
        {
            Destroy(texts[0].gameObject);

            texts[0].StartDestroying(0f, 0.2f);
            texts.Remove(texts[0]);
        }
    }

    private void MoveNumbersAfterCharacter()
    {
        foreach (DamageText text in texts)
        {
            float x = transform.position.x + text.distance * Mathf.Cos(text.angle * Mathf.Deg2Rad);
            float y = transform.position.y + text.distance * Mathf.Sin(text.angle * Mathf.Deg2Rad);
            text.transform.position = Camera.main.WorldToScreenPoint(new Vector3(x, y, -1f));
        }
    }

    public void ShowDamage(float damage, LivingEntity entity, bool displayNumbers)
    {
        if (damage >= 1f && displayNumbers)
        {
            int angle = Random.Range(80, 100);
            float distance = Random.Range(entity.transform.localScale.y, entity.transform.localScale.y);
            float x = transform.position.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = transform.position.y + distance * Mathf.Sin(angle * Mathf.Deg2Rad);

            DamageText text = Instantiate(damageTextPrefab, Camera.main.WorldToScreenPoint(new Vector3(x, y, -1f)), Quaternion.identity);
            text.transform.SetParent(parentObjectTransform, true);
            text.GetComponent<TextMeshProUGUI>().text = ((int)Mathf.Ceil(damage)).ToString();
            text.SetDamageDisplay(this);
            text.angle = angle;
            text.distance = distance;

            texts.Add(text);
            text.StartDestroying(0.3f, 0.3f);
        }

            StartCoroutine(DamageColoring(entity.GetComponent<SpriteRenderer>()));
    }

    private IEnumerator DamageColoring(SpriteRenderer renderer)
    {
        renderer.color = new Color(1f, 0.7f,0.7f, 1f);
        yield return new WaitForSeconds(0.05f);
        renderer.color = Color.white;
    }


    private void OnDestroy()
    {
        foreach (DamageText text in texts)
        {
            text.StartDestroying(0.1f, 0.2f);
        }
    }
}
