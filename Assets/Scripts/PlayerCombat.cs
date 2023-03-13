using Newtonsoft.Json.Bson;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //config
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayers;
    public Vector2 AttackDash;

    //cached
    [SerializeField] private int attackDamage = 1;
    private Animator _animator;

    //state
    public bool isAttacking = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            // Detects if player attacking or not
            Attack();
        }
    }

    private void Attack()
    {
        _animator.SetTrigger("IsAttacking");
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage); // Damaging enemies
        }

        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        // Coroutine that don't let player spam attack
        isAttacking = true;
        yield return new WaitForSecondsRealtime(0.8f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) // if there is no object function won't work
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // draw attack range 
    }
}
