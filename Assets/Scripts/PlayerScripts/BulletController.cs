using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    public static event Action<BulletController, Collider2D> OnBulletCollidesEnemy;

    public float _lifetime;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private TrailRenderer _trailRenderer;



    public void Initialize(Vector3 position, Quaternion rotation, float bulletSpeed)
    {
        _lifetime = 0f;

        _trailRenderer.Clear();

        transform.SetPositionAndRotation(position, rotation);

        gameObject.SetActive(true);

        _rigidbody.linearVelocity = transform.up * bulletSpeed;
    }

    public void Terminate()
    {
        _rigidbody.linearVelocity = Vector2.zero;

        gameObject.SetActive(false);
    }



    public void StopMovement()
    {
        _rigidbody.linearVelocity = Vector2.zero;
    }

    public void StartMovement(float bulletSpeed)
    {
        _rigidbody.linearVelocity = transform.up * bulletSpeed;
    }



    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Enemy"))
        {
            OnBulletCollidesEnemy?.Invoke(this, otherCollider);
        }
    }
}
