using UnityEngine;

public class Transition : MonoBehaviour
{
    private Rigidbody2D _playerRB;
    private Animator _animator;
    private Vector2 _playerVelocity;
    private PLayer player;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerRB = GetComponent<Rigidbody2D>();
        player = GetComponent<PLayer>();
    }
    void Update()
    {
        Run();
    }
    private void Run()
    {
        _playerVelocity.x = 6;
        _playerVelocity.y = _playerRB.velocity.y;
        _playerRB.velocity = _playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_playerRB.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }
}
