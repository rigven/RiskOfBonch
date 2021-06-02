using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : InteractiveObject
{
    public Boss bossPrefab;
    [SerializeField] public GameObject portalVFX;
    public Vector3 bossSpawnPos;

    new private void Start()
    {
        base.Start();
        labelText = "[F] - Призвать босса";
        labelObj.text = labelText;
    }

    protected override void UseObject()
    {
        if (numberOfUses == 0)
        {
            StartCoroutine(SpawnBoss());
            labelObj.gameObject.SetActive(false);
            locked = true;
        }
        if (numberOfUses == 1)
        {
            if (SceneManager.GetActiveScene().name == "Level 0")
            {
                FindObjectOfType<SceneLoader>().LoadManualNextScene();
            } 
            else
            {
                FindObjectOfType<SceneLoader>().LoadNextScene();
            }
        }

        numberOfUses++;
    }

    private IEnumerator SpawnBoss()
    {
        GameObject portalVFX = Instantiate(this.portalVFX, bossSpawnPos, Quaternion.identity);
        portalVFX.transform.localScale = new Vector3(20, 20, 1);

        yield return new WaitForSeconds(0.5f);

        Boss boss = Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity);
        if (boss.GetComponent<SausageBoss>()) Destroy(FindObjectOfType<EnemySpawner>());
        boss.gameObject.transform.parent = gameObject.transform;
    }

    public void SwitchToNextLevel()
    {
        GetComponent<Animator>().SetBool("Active", true);
        labelText = "[F] - Cледующий уровень";
        labelObj.text = labelText;
        locked = false;
        if (GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            canUse = true;
            labelObj.gameObject.SetActive(true);
        }
    }
}
