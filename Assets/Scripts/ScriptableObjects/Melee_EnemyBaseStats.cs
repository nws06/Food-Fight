using UnityEngine;

[CreateAssetMenu(fileName = "Melee_EnemyBaseStats", menuName = "Scriptable Objects/Melee_EnemyBaseStats")]
public class Melee_EnemyBaseStats : ScriptableObject
{
    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private float _baseHealth;

    [SerializeField] private float _baseDamage;



    public float BaseMoveSpeed { get => _baseMoveSpeed; }
    public float BaseHealth { get => _baseHealth; }

    public float BaseDamage { get => _baseDamage; }
}
