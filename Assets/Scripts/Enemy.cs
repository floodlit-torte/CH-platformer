using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    //config
    [SerializeField] private float EnemySpeed = 1f;
    [SerializeField] private Vector2 DyingKick;

    //cached
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private int currentHealth;
    private Rigidbody2D _enemyRB;
    private Animator _animator;

    //state
    private bool isDead = false;

    private void Start()
    {
        _enemyRB = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (IsFacindRight())
        {
            _enemyRB.velocity = Vector3.right * EnemySpeed;
        }
        else
        {
            _enemyRB.velocity = Vector3.left * EnemySpeed;
        }
    }

    private bool IsFacindRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector3(-(Mathf.Sign(_enemyRB.velocity.x)), 1f, 1f);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        DyingKick.x *= -Mathf.Sign(_enemyRB.velocity.x);
        _enemyRB.velocity = DyingKick;
        _animator.SetTrigger("TakedDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Enemy Died!");
        _animator.SetBool("IsDead", true);
        DyingKick.x *= -Mathf.Sign(_enemyRB.velocity.x);
        _enemyRB.velocity = DyingKick;
        Destroy(gameObject);
    }
}
