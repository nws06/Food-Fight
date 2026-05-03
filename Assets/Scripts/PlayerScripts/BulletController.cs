using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private PlayerBaseStats _baseStats;
    private float _bulletSpeed;
    private Rigidbody2D _rigidbody;



    void Awake()
    {
        _bulletSpeed = _baseStats.BaseBulletSpeed;

        _rigidbody = GetComponent<Rigidbody2D>();
    }



    void FixedUpdate()
    {
        _rigidbody.linearVelocity = transform.up * _bulletSpeed;
    }
}