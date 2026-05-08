using UnityEngine;

[System.Serializable]
public class RuntimePlayerStats 
{
    [SerializeField] private PlayerBaseStats _baseStats;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;

    [Header("Health")]
    [SerializeField] private float _maxHealth;

    [Header("Attack")]
    [SerializeField] private float _damage;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _shotCooldown;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _bulletLifetime;

    private float _minShotCooldown = 0.05f;
    private float _minReloadTime = 0.05f;



    public float MoveSpeed { get => _moveSpeed; }

    public float MaxHealth { get => _maxHealth; }

    public float Damage { get => _damage; }
    public float ReloadTime { get => _reloadTime; }
    public float ShotCooldown { get => _shotCooldown; }
    public float BulletSpeed { get => _bulletSpeed; }
    public int MaxAmmo { get => _maxAmmo; }
    public float BulletLifetime { get => _bulletLifetime; }



    public RuntimePlayerStats(PlayerBaseStats playerBaseStats)
    {
        _baseStats = playerBaseStats;

        Initialize();
    }

    void Initialize()
    {
        _moveSpeed = _baseStats.BaseMoveSpeed;

        _maxHealth = _baseStats.BaseHealth;

        _damage = _baseStats.BaseDamage;
        _reloadTime = _baseStats.BaseReloadTime;
        _shotCooldown = _baseStats.BaseShotCooldown;
        _bulletSpeed = _baseStats.BaseBulletSpeed;
        _maxAmmo = _baseStats.BaseMaxAmmo;
        _bulletLifetime = _baseStats.BaseBulletLifetime;
    }



    // NOTE: VERIFY THAT THIS IMPLEMENTATION IS RIGHT
    public void ApplyUpgrade(Upgrade upgrade)
    {
        if (upgrade.TryAcquire()) 
        {
            float value = upgrade.TotalValue;

            switch (upgrade.Data.Type)
            {
                case UpgradeType.MoveSpeed:
                    _moveSpeed = _baseStats.BaseMoveSpeed * (1 + value);
                    break;
                case UpgradeType.MaxHealth:
                    _maxHealth = _baseStats.BaseHealth + value;
                    break;
                case UpgradeType.Damage:
                    _damage = _baseStats.BaseDamage * (1 + value);
                    break;
                case UpgradeType.ReloadTime:
                    _reloadTime = Mathf.Max(_minReloadTime, _baseStats.BaseReloadTime * (1 + value));
                    break;
                case UpgradeType.ShotCooldown:
                    _shotCooldown = Mathf.Max(_minShotCooldown, _baseStats.BaseShotCooldown * (1 + value));
                    break;
                case UpgradeType.BulletSpeed:
                    _bulletSpeed = _baseStats.BaseBulletSpeed * (1 + value);
                    break;
                case UpgradeType.MaxAmmo:
                    _maxAmmo = Mathf.RoundToInt(_baseStats.BaseMaxAmmo * (1 + value));
                    break;
                case UpgradeType.BulletLifetime:
                    _bulletLifetime = _baseStats.BaseBulletLifetime * (1 + value);
                    break;
                case UpgradeType.DefaultUpgradeType:
                    Debug.LogWarning("RuntimePlayerStats.cs: Default upgrade was applied.");
                    break;
                default:
                    Debug.LogError("RuntimePlayerStats.cs: Invalid upgrade was applied.");
                    break;
            }
        }
    }
}