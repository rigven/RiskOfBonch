using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : LimitedBonus
{
    public bool isEnabled = false;

    private float healthPercent = 0.05f;

    private float startingCooldown = 10f;
    private float minCooldown = 5f;
    private float startingDuration = 0.5f;
    private float maxDuration = 2f;

    private float cooldownCoeff;
    private float durationCoeff;

    [SerializeField] private float cooldown;
    [SerializeField] private float duration;

    private bool isAvailable = true;

    private Player player;
    private SpriteRenderer bodySpriteRenderer;
    private SpriteRenderer rightLegSpriteRenderer;
    private SpriteRenderer leftLegSpriteRenderer;
    private Color invisColor;
    private Color normalColor;

    new protected void Start()
    {
        player = GetComponent<Player>();
        player.AddInvisibilityModifier(this);

        bodySpriteRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
        rightLegSpriteRenderer = transform.Find("Legs").Find("Right Leg").GetComponent<SpriteRenderer>();
        leftLegSpriteRenderer = transform.Find("Legs").Find("Left Leg").GetComponent<SpriteRenderer>();
        invisColor = new Color(1f, 1f, 1f, 0.5f);
        normalColor = new Color(1f, 1f, 1f, 1f);

        cooldown = startingCooldown;
        duration = startingDuration;
        CalculateCoeffs();
    }

    private void Update()
    {
    }

    private void CalculateCoeffs()
    {
        cooldownCoeff = CalculateProgressionCoeff(startingCooldown, minCooldown);
        durationCoeff = CalculateProgressionCoeff(startingDuration, maxDuration);
    }

    public override void AddModification()
    {
        ProgressValue(ref duration, startingDuration, durationCoeff);
        ProgressValueDown(ref cooldown, minCooldown, cooldownCoeff);
        modificationsNumber++;
        print("add invis");
        print(duration);
    }

    public void HandleDamage(float damage)
    {
        if (isAvailable && isEnabled)
        {
            if (damage >= player.GetMaxHealth() * healthPercent)
            {
                StartCoroutine(SetInvis());
            }
        }
    }

    private IEnumerator SetInvis()
    {
        player.isInvisible = true;
        bodySpriteRenderer.color = invisColor;
        rightLegSpriteRenderer.color = invisColor;
        leftLegSpriteRenderer.color = invisColor;

        yield return new WaitForSeconds(duration);

        player.isInvisible = false;
        bodySpriteRenderer.color = normalColor;
        rightLegSpriteRenderer.color = normalColor;
        leftLegSpriteRenderer.color = normalColor;

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(cooldown);
        isAvailable = true;
    }


}
