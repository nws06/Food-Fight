using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    public static event Action<BulletController, Collider2D> OnBulletCollidesEnemy;
    public Rigidbody2D _rigidbody;
    public float _spawnTime;



    void Awake()
    {
        _spawnTime = Time.time;
    }



    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Enemy"))
        {
            OnBulletCollidesEnemy?.Invoke(this, otherCollider);
        }
    }
}
