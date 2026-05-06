using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour, IPauseableFixedUpdate
{
    [SerializeField] private PlayerBaseStats _baseStats;
    private float _bulletSpeed;
    private Rigidbody2D _rigidbody;



    void Awake()
    {
        _bulletSpeed = _baseStats.BaseBulletSpeed;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        UpdateManager.Instance.RegisterForFixedUpdate(this);

        PauseManager.OnGamePause += OnGamePause;
    }



    public void OnPauseableFixedUpdate(float deltaTime)
    {
        _rigidbody.linearVelocity = transform.up * _bulletSpeed;
    }

    void OnGamePause()
    {
        _rigidbody.linearVelocity = Vector2.zero;
    }



    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Enemy"))
        {
            Melee_EnemyManager.Instance.DamageEnemy(otherCollider.GetComponent<Melee_EnemyController>(), _baseStats.BaseDamage);
        }
    }



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromFixedUpdate(this);

        PauseManager.OnGamePause -= OnGamePause;
    }
}