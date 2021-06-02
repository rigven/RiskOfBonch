using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectiveShield : LimitedBonus
{
    private float startingChance = 0.1f;
    private float maxChance = 0.2f;
    private float maxRotationDegrees = 30;

    private float chanceCoeff;
    [SerializeField] private float chance;

    new protected void Start()
    {
        chance = startingChance;
        chanceCoeff = CalculateProgressionCoeff(startingChance, maxChance);
    }

    public void HandleCollision(Bullet bullet)
    {
        bool reflection = UnityEngine.Random.Range(0f, 1f) <= chance;

        if (reflection)
        {
            bullet.source = ProjectileSource.Player;
            bullet.ChooseCollisionLayers();
            float rotationRad = UnityEngine.Random.Range(-maxRotationDegrees, maxRotationDegrees) * Mathf.Deg2Rad;

            Vector2 reflectedVelocity = -bullet.GetComponent<Rigidbody2D>().velocity;
            float newX = reflectedVelocity.x * Mathf.Cos(rotationRad) - reflectedVelocity.y * Mathf.Sin(rotationRad);
            float newY = reflectedVelocity.x * Mathf.Sin(rotationRad) + reflectedVelocity.y * Mathf.Cos(rotationRad);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(newX, newY);
            bullet.TurnTowardsVelocity();
        }
    }

    public override void AddModification()
    {
        ProgressValue(ref chance, startingChance, chanceCoeff);
        modificationsNumber++;
    }
}
