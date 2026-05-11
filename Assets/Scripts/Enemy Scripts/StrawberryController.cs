using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StrawberryController : MonoBehaviour, IEnemy
{
    [field: SerializeField] public bool IsAlive { get; private set; }
    public Action<IEnemy> OnDeath { get; set; }
    [field: SerializeField] public float CurrentHp { get; private set; }
    public Rigidbody2D EnemyRigidbody { get; private set; }



    void Awake()
    {
        if (EnemyRigidbody == null)
            EnemyRigidbody = GetComponent<Rigidbody2D>();
    }



    public void Spawn(Vector2 position, float rotation, Transform parent, float hp, float size = 1f, bool active = true)
    {
        IsAlive = true;
        CurrentHp = hp;
        transform.SetParent(parent);
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotation));
        transform.localScale = Vector3.one * size;
        gameObject.SetActive(active);
    }

    public void Reset()
    {
        IsAlive = false;
        CurrentHp = 0f;
        EnemyRigidbody.linearVelocity = Vector2.zero;
        transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }



    public void SetMovement(float velocity)
    {
        EnemyRigidbody.linearVelocity = transform.up * velocity;
    }



    public void Damage(float damage)
    {
        if (!IsAlive) 
            return;

        CurrentHp -= damage;

        if (CurrentHp <= 0)
            Kill();
    }

    public void Kill(bool destroyObject = false)
    {
        // Death animation
        // Spawn XP

        IsAlive = false;
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
