using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _healtMax, _speed, _jumpSpeed;
    
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadious;

    [SerializeField] private Transform _gun;
    [SerializeField] private GameObject _bullet;

    private float _health;

    private Rigidbody2D _rb;
    private Camera _cam;

    private bool IsGrounded =>
        Physics2D.OverlapCircle(transform.position + Vector3.down * 0.5f, _groundCheckRadious, _groundLayer);

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;

        _health = _healtMax;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy")) return;
        OnTakeDamage(new Damage(1f, collision.transform));
    }

    private void Shoot()
    {
        var bullet = Instantiate(_bullet, _gun.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(1, _gun.right);
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
            _rb.AddForce(new Vector2(0, _jumpSpeed * 100));

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();

        var dir = Input.mousePosition - _cam.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _gun.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _gun.transform.localScale = new Vector3(-1, 1, 1);
        }

        var rotation = _gun.rotation;
        _gun.transform.localScale = new Vector3(transform.localScale.x,
            rotation.z > -0.7 && rotation.z < 0.7 ? 1 : -1, 1);
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(horizontal * _speed, _rb.velocity.y + (IsGrounded ? 0 : -0.75f));
    }
    private void OnTakeDamage(Damage damage)
    {
        _health -= damage.amount;
        Debug.Log("Take Damage HP: " + _health);

        _rb.AddForce(new Vector2(500 * (damage.damageDealer.position.x < transform.position.x ? 1 : -1), 100));
        if (_health <= 0)
        {
            Debug.Log("Game over!!!");
            Destroy(gameObject);
        }
    }
}
