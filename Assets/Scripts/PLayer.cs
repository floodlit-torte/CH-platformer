using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PLayer : MonoBehaviour
{
    [SerializeField] private TMP_Text LanternScoreText;

    //config
    [SerializeField] private float playerSpeed = 6f;
    [SerializeField] private float playerClimbSpeed = 5f;
    [SerializeField] private float playerJumpSpeed = 6.5f;
    [SerializeField] private Vector2 DyingKick;
    [SerializeField] private float invincibilityTime = 4f;

    //state
    private bool isAlive = true;
    private bool isTakingDamage = false;
    private bool CanDoubleJump = true;

    //cached
    private int _LanternScore = 0;
    private Rigidbody2D _playerRB;
    private Animator _animator;
    private Collider2D _playerBodyCollider;
    private BoxCollider2D _playerFeetCollider;
    private Vector2 _playerVelocity;
    private Vector3 _playerLocalScale;
    private float _playerStarterGravityScale;
    private PlayerCombat playerCombat;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerRB = GetComponent<Rigidbody2D>();
        _playerBodyCollider = GetComponent<Collider2D>();
        _playerFeetCollider = GetComponent<BoxCollider2D>();
        _playerLocalScale = transform.localScale;
        _playerStarterGravityScale = _playerRB.gravityScale;
        playerCombat = GetComponent<PlayerCombat>();
    }
    private void Update()
    {
        if (!isAlive)
            return;
        Run();
        Jump(); 
        DoubleJump();
        if (_playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _animator.SetBool("IsJumping", false);
        }
        FlipSprite();
        //ClimbingOnLadder();
        if (!isTakingDamage)
        {
            Die();
        }
        if (playerCombat.isAttacking)
        {
            if (transform.localScale.x < 0 && playerCombat.AttackDash.x > 0 || transform.localScale.x > 0 && playerCombat.AttackDash.x < 0)
                playerCombat.AttackDash.x *= -1;
            _playerRB.velocity = playerCombat.AttackDash;
            StartCoroutine(TakeDamage());
        }
    }

    private void Die()
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            //_animator.SetTrigger("Dying");
            DyingKick.x *= -Mathf.Sign(_playerRB.velocity.x);
            _playerRB.velocity = DyingKick;
            _animator.SetTrigger("IsTakingDamage");
            FindObjectOfType<GameSession>().ProcessDeath();
            StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage()
    {
        //Coroutine that makes player invincible 
        isTakingDamage = true;
        yield return new WaitForSeconds(invincibilityTime);
        isTakingDamage = false;
    }

    private void Run()
    {
        if (playerCombat.isAttacking)
            return;
        float horizontalMovement = Input.GetAxis("Horizontal");
        _playerVelocity.x = horizontalMovement * playerSpeed;
        _playerVelocity.y = _playerRB.velocity.y;
        _playerRB.velocity = _playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRB.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        if (playerCombat.isAttacking)
            return;
        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRB.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            _playerLocalScale.x = Mathf.Sign(_playerRB.velocity.x);
            transform.localScale = _playerLocalScale;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !playerCombat.isAttacking)
        {
            _playerVelocity.x = _playerRB.velocity.x;
            _playerVelocity.y = playerJumpSpeed;
            _playerRB.velocity = _playerVelocity;
            CanDoubleJump = true;
            _animator.SetBool("IsJumping", true);
        }
    }
    private void DoubleJump()
    {
        if(Input.GetButtonDown("Jump") && !_playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && CanDoubleJump)
        {
            _playerVelocity.x = _playerRB.velocity.x;
            _playerVelocity.y = playerJumpSpeed;
            _playerRB.velocity = _playerVelocity;
            CanDoubleJump = false;
        }
    }

    //private void ClimbingOnLadder()
    //{
    //    if (!_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
    //    {
    //        _animator.SetBool("IsClimbing", false);
    //        _playerRB.gravityScale = _playerStarterGravityScale;
    //        return;
    //    }
    //    _animator.SetBool("IsRunning", false);
    //    float verticalMovement = Input.GetAxis("Vertical");
    //    _playerVelocity.x = _playerRB.velocity.x;
    //    _playerVelocity.y = verticalMovement * playerClimbSpeed;
    //    _playerRB.velocity = _playerVelocity;
    //    _playerRB.gravityScale = 0f;

    //    bool playerHasVerticalSpeed = Mathf.Abs(_playerRB.velocity.y) > Mathf.Epsilon;
    //    _animator.SetBool("IsClimbing", playerHasVerticalSpeed);
    //}

    public void CollectLantern()
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Lantern")))
        {
            _LanternScore++;
            LanternScoreText.text = $"Lanterns: {_LanternScore}";
        }
    }
}
