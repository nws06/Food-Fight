using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee_EnemyBaseStats", menuName = "Scriptable Objects/Melee_EnemyBaseStats")]
public class Melee_EnemyBaseStats : ScriptableObject
{
    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private float _baseHealth;

    [SerializeField] private float _baseDamage;

    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;



    public float BaseMoveSpeed { get => _baseMoveSpeed; }
    public float BaseHealth { get => _baseHealth; }

    public float BaseDamage { get => _baseDamage; }

    public float MinSize { get => _minSize; }
    public float MaxSize { get => _maxSize; }
}
