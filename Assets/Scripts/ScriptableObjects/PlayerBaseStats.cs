using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "Scriptable Objects/PlayerBaseStats")]
public class PlayerBaseStats : ScriptableObject
{
    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private float _baseHealth;

    [SerializeField] private float _baseDamage;
    [SerializeField] private float _baseReloadTime;
    [SerializeField] private float _baseShotCooldown;
    [SerializeField] private float _baseBulletSpeed;
    [SerializeField] private int _baseMaxAmmo;
    [SerializeField] private float _baseBulletLifetime;



    public float BaseMoveSpeed { get => _baseMoveSpeed; }
    public float BaseHealth { get => _baseHealth; }

    public float BaseDamage { get => _baseDamage; }
    public float BaseReloadTime { get => _baseReloadTime; }
    public float BaseShotCooldown { get => _baseShotCooldown; }
    public float BaseBulletSpeed { get => _baseBulletSpeed; }
    public int BaseMaxAmmo { get => _baseMaxAmmo; }
    public float BaseBulletLifetime { get => _baseBulletLifetime; }
}
