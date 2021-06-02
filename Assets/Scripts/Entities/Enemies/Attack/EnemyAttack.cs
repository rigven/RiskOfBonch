using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyAttack : MonoBehaviour
{
    protected Enemy enemy;

    [Header("Атака")]
    [SerializeField] public float damage = 5;
    [SerializeField] public float attackDistance = 2;
    [SerializeField] public float safeAttackDistance = 1.5f;
    [SerializeField] public float attackCooldown = 2;

    protected bool canAttack = true;


    protected void Start()
    {
        enemy = GetComponent<Enemy>();
        damage *= DifficultyController.difficulty;
    }

    protected void Update() { }

    abstract public void Attack();

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
