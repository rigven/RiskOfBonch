using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    public Player targetPlayer;
    public EnemySpawner spawner;
    protected EnemyMovement enemyMovement;

    [Header("Награда")]
    [SerializeField] public int killPoints = 1;
    [SerializeField] protected Modifier modifierPref;
    [NonSerialized] public float lootDropChance = 0.1f;
    [SerializeField] private Dictionary<ModifierType, int> dropDict = new Dictionary<ModifierType, int>();
    [SerializeField] public List<ModifierType> dropList = new List<ModifierType>();
    [SerializeField] public List<int> dropChances = new List<int>();

    [Header("Оформление")]
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected ParticleSystem deathVFX;
    protected float deathSoundVolume = 2f;

    private bool isElite = false;
    private bool isDead = false;

    public float distanceToDestroy = 40f;


    new protected void Awake()
    {
        maxHealth *= DifficultyController.difficulty;
        base.Awake();

        killPoints *= (int) DifficultyController.difficulty;

        if (usesHealthBar)
        {
            healthBar = transform.Find("HPbar").GetComponent<HealthBar>();
        }
        targetPlayer = FindObjectOfType<Player>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        if (dropChances.Count == 0)
        {
            dropList.Clear();
            dropChances.Clear();
            dropList.Add(ModifierType.Heal);
            dropChances.Add(2); 
        }
        if (!isElite)
        {
            for (int i = 0; i < dropChances.Count; i++)
            {
                dropDict.Add(dropList[i], dropChances[i]);
            }
        }
    }

    new protected void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        if (!isDead)
        {
            isDead = true;
            ScoreController.score += killPoints;
            DropLoot();
            AudioSource.PlayClipAtPoint(deathSound, FindObjectOfType<Player>().transform.position, deathSoundVolume);
            if (deathVFX)
            {
                ParticleSystem dhvfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
                if (GetComponent<RotatingFlyingEnemy>())
                {
                    dhvfx.startRotation = transform.localEulerAngles.z / 55.55f;        // Работает, но по возможности сделать нормально
                    dhvfx.transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
                else
                {
                    dhvfx.transform.localScale = transform.localScale;
                }
            }
            spawner.mobCount--;
            Destroy(gameObject);
        }
    }

    public override void HandleDamage(float damage)
    {
        if (!enemyMovement.pursuit && !targetPlayer.isInvisible)
        {
            enemyMovement.pursuit = true;
            enemyMovement.wanders = false;
        }
        RecieveDamage(damage);
    }

    public void DropLoot()
    {
        if (dropDict.Count != 0)
        {
            float ran = UnityEngine.Random.Range(1, 102);
            if (ran <= (int)(lootDropChance * 100))
            {
                Modifier modifier = Instantiate(modifierPref, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                modifier.dropList = dropDict;
                modifier.GenModifierType(); 
            }
        }
    }

    public void SetLoot(Dictionary<ModifierType, int> dropList)
    {
        isElite = true;
        // dropDict = dropList;   // Передаёт указатель, а не копию объекта, в итоге все элитные мобы работают с одним объектом, и когда в старте
        // еще добавляются модификаторы, вылезает "ArgumentException: An item with the same key has already been added. Key".
        // Наверное, можно оставить тут указатель для экономии памяти, но только если список будет одинаковым для всех.

        // Не знаю, чем конкретно было вызвано отсутствие лута у элитных, но сейчас я довольно долго поиграла и такого не видела, так что возможно этим.
        // Можно написать автотест, чтоб убедиться, что такого точно больше не будет.
        dropDict = new Dictionary<ModifierType, int>(dropList);
    }

}
