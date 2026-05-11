using System;
using UnityEngine;

public interface IEnemy
{
    public bool IsAlive { get; }
    public Action<IEnemy> OnDeath { get; set; }
    public float CurrentHp { get; }
    public Rigidbody2D EnemyRigidbody { get; }



    public void Spawn(Vector2 position, float rotation, Transform parent, float hp, float size = 1f, bool active = true);

    public void Reset();

    public void SetMovement(float velocity);

    public void Damage(float damage);

    public void Kill(bool destroyObject = false);

    public void Destroy();
}
