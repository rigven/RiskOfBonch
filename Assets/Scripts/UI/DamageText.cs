using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public int angle;
    public float distance;
    private DamageDisplay damageDisplay;
    private bool fading = false;
    private float fadingTime;
    private float lifetime;
    private TextMeshProUGUI tmp;


    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (fading)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - (255 * Time.deltaTime / fadingTime)/100f);
        }
        if (tmp.color.a == 0)
        {
            Destroy(this);
        }
    }

    public void StartDestroying(float lifetime, float fadingTime)
    {
        this.lifetime = lifetime;
        this.fadingTime = fadingTime;
        StartCoroutine(StartDestroying());
    }

    private IEnumerator StartDestroying()
    {
        yield return new WaitForSeconds(lifetime);

        fading = true;
    }

    public void SetDamageDisplay(DamageDisplay damageDisplay)
    {
        this.damageDisplay = damageDisplay;
    }
     
    private void OnDestroy()
    {
        damageDisplay.texts.Remove(this);
    }
}
