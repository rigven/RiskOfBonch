using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] Portal portalPrefab;
    [SerializeField] Boss boss;
    [SerializeField] GameObject worldObjects;

    void Start()
    {
        GameObject spawnPointsParent = transform.Find("SpawnPoints").gameObject;
        Transform spawnPoint = spawnPointsParent.transform.GetChild(UnityEngine.Random.Range(0, spawnPointsParent.transform.childCount)).transform;
        Portal portal = Instantiate(portalPrefab, spawnPoint);
        portal.bossSpawnPos = spawnPoint.Find("BossSpawnPoint").position;
        portal.bossPrefab = boss;
        portal.transform.parent = worldObjects.transform;
    }
}
