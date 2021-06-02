using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    [SerializeField] private float timeInterval = 5f;
    [SerializeField] private float difficultyIncrement = 0.0025f;
    [SerializeField] private float spawnIntensityIncrement = 0.01f;
    public static float defaultDifficulty = 1f;
    public static float difficulty = defaultDifficulty;
    public static float spawnIntensity;
    public bool increase = true;
    public static bool isBossFight = false;

    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        if (!GamePause.isPaused)
        {
            if (increase && !isBossFight)
            {
                StartCoroutine(IncreaseDifficulty());
            }
        }
    }

    private IEnumerator IncreaseDifficulty()
    {
        increase = false;
        yield return new WaitForSeconds(timeInterval);
        difficulty += difficultyIncrement;
        spawnIntensity = spawnIntensity + spawnIntensityIncrement;
        if (enemySpawner) enemySpawner.UpdateCooldown();
        increase = true;
    }
}
