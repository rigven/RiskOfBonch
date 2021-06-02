using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Specifications")]

    [SerializeField] private float spawnCooldownMin = 1.5f;
    [SerializeField] private float spawnCooldownMax = 3f;
    //private float spawnCooldownDefault = 2f;
    private float spawnCooldown;
    [SerializeField] private int spawnMaxRadiusY = 14;
    [SerializeField] private int spawnMaxRadiusX = 28;
    private float safeZone;
    private bool spawn = true;

    [Header("Enemies")]
    [SerializeField] Enemy[] enemyPrefabs;
    [SerializeField] int[] enemySpawnWeight;

    [Header("Decoration")]
    [SerializeField] protected GameObject portalEffectPrefab;

    private Player player;
    [SerializeField] private Transform enemyParentTransform;
    private int maxChance;
    [SerializeField] public Material[] outlineMaterial;
    [SerializeField] public float eliteMobModifier = 1.5f;
    [SerializeField] private Dictionary<ModifierType, int> eliteDropDict = new Dictionary<ModifierType, int>();
    [SerializeField] public List<ModifierType> eliteDropList = new List<ModifierType>();
    [SerializeField] public List<int> eliteDropChance = new List<int>();
    private int maxMobCount = 20;
    [NonSerialized] public int mobCount = 0;
    private int eliteProportion = 16;

    void Start()
    {
        for (int i = 0; i < eliteDropList.Count; i++)
        { 
            eliteDropDict.Add(eliteDropList[i], eliteDropChance[i]);
        }
        DifficultyController.spawnIntensity = 1f;
        spawnCooldown = spawnCooldownMax;
        for (int i = 0; i < enemySpawnWeight.Length; i++)
            maxChance += enemySpawnWeight[i];
        player = FindObjectOfType<Player>();
        safeZone = spawnMaxRadiusX / 8 * 3;
    }

    void Update()
    {
        if (spawn && !GamePause.isPaused)
        {
            SpawnMobs();
            StartCoroutine(StartWave());
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(spawnCooldown);
        spawn = true;
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position;
    }

    private void SpawnMobs()
    {
        for (int i = 0; i < 1; i++)
        {
            if (mobCount < maxMobCount)
            {
                StartCoroutine(SpawnMob());
                mobCount++;
            }
        }

        spawn = false;
    }

    private IEnumerator SpawnMob()
    {
        float x;
        float y;
        int mask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D raycastHit;
        Collider2D colliderHit;
        do
        {
            x = UnityEngine.Random.Range(player.transform.position.x - spawnMaxRadiusX, player.transform.position.x + spawnMaxRadiusX) + 0.5f;
            y = UnityEngine.Random.Range(player.transform.position.y - spawnMaxRadiusY, player.transform.position.y + spawnMaxRadiusY);

            raycastHit = Physics2D.Raycast(new Vector2(x, y), Vector2.down, spawnMaxRadiusY, mask);
            colliderHit = Physics2D.OverlapPoint(raycastHit.point + new Vector2(0, 0.5f), mask);
        } while (Vector2.Distance(raycastHit.point, transform.position) <= safeZone || colliderHit || raycastHit.collider == null);

        GameObject portalEffect = Instantiate(portalEffectPrefab, raycastHit.point + new Vector2(0, 0.5f), Quaternion.identity);
        Destroy(portalEffect, 2f);
        yield return new WaitForSeconds(0.5f);
        int a = UnityEngine.Random.Range(1, maxChance+1);
        int chance = maxChance;
        for (int i = enemySpawnWeight.Length- 1; i >= 0; i--)
        {
            chance = chance - enemySpawnWeight[i];
            if (chance < a)
            {
                a = i;
                break;
            }
        }
        Enemy enemy = Instantiate(enemyPrefabs[a], raycastHit.point + new Vector2(0, 0.5f), Quaternion.identity);
        enemy.transform.parent = enemyParentTransform;
        enemy.spawner = this;
        if (UnityEngine.Random.Range(0, eliteProportion) == 0)
            UpgradeEliteMob(enemy, a);
    }
    private void UpgradeEliteMob(Enemy enemy, int a)
    {
        enemy.SetMaxHealth(enemy.GetMaxHealth()*eliteMobModifier*eliteMobModifier);
        enemy.SetHealth(enemy.GetMaxHealth());
        enemy.GetComponent<EnemyAttack>().damage *= eliteMobModifier;
        enemy.SetLoot(eliteDropDict);
        enemy.lootDropChance = 1f;
        enemy.killPoints *= (int) eliteMobModifier;
        enemy.gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1);

        SpriteRenderer sp = enemy.gameObject.GetComponent<SpriteRenderer>();
        if (outlineMaterial[a])
        {
            sp.material = outlineMaterial[a];
        }
        sp.material.SetVector("_OutlineColor", UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f)* 1.5f);
        sp.material.SetColor("_OutlineColor_1", UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f)* 1.5f);
    }

    public void UpdateCooldown()
    {
        spawnCooldown = Mathf.Clamp(spawnCooldownMax / Mathf.Pow(DifficultyController.spawnIntensity, 1 / 3f), spawnCooldownMin, spawnCooldownMax);
        //print(spawnCooldown);
    }
}
