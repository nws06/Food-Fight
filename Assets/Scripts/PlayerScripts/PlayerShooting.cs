using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Pool;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour, IPauseableUpdate, IPauseableFixedUpdate
{
    [SerializeField] private PlayerBaseStats _baseStats;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _bulletPoolParent;
    [SerializeField] private BulletController _bulletPrefab;
    private float _damage;
    private int _currentAmmo;
    private int _maxAmmo;
    private float _reloadTime;
    private float _bulletSpeed;
    private float _shotCooldown;
    private float _bulletLifetime;
    private bool _isReloading;
    private float _lastShotTime;
    private InputAction _shootAction;
    private InputAction _reloadAction;
    private Coroutine _reloadCoroutine;
    private ObjectPool<BulletController> _bulletPool;
    private List<BulletController> _activeBullets = new List<BulletController>();



    void Awake()
    {
        PauseManager.OnGamePause += OnGamePause;

        _damage = _baseStats.BaseDamage;
        _maxAmmo = _baseStats.BaseMaxAmmo;
        _reloadTime = _baseStats.BaseReloadTime;
        _bulletSpeed = _baseStats.BaseBulletSpeed;
        _shotCooldown = _baseStats.BaseShotCooldown;
        _bulletLifetime = _baseStats.BaseBulletLifetime;

        _currentAmmo = _maxAmmo;

        _lastShotTime = -5f;

        _shootAction = InputSystem.actions.FindAction("Attack");
        _reloadAction = InputSystem.actions.FindAction("Reload");

        _bulletPool = new ObjectPool<BulletController>(
            createFunc: CreateBullet,
            actionOnGet: GetBullet,
            actionOnRelease: ReleaseBullet,
            actionOnDestroy: DestroyBullet,
            collectionCheck: true,   
            defaultCapacity: 60,
            maxSize: 1080
        );
    }

    void Start()
    {
        UpdateManager.Instance.RegisterForUpdate(this);

        PauseManager.OnGamePause += OnGamePause;
        PauseManager.OnGameUnpause += OnGameUnpause;
        BulletController.OnBulletCollidesEnemy += BulletHitEnemy;
    }



    public void OnPauseableUpdate(float deltaTime)
    {
        if (_shootAction.IsPressed() && !_isReloading && Utils.IsOffCooldown(_lastShotTime, _shotCooldown))
        {
            if (_currentAmmo > 0)
                Shoot();
            else if (!_isReloading)
                _reloadCoroutine = StartCoroutine(Reload()); 
        }

        if (_reloadAction.IsPressed() && !_isReloading && _currentAmmo < _maxAmmo)
            _reloadCoroutine = StartCoroutine(Reload()); 
        
        
        for (int i = _activeBullets.Count - 1; i >= 0; i--)
        {
            if (Time.time - _activeBullets[i]._spawnTime >= _bulletLifetime)
                _bulletPool.Release(_activeBullets[i]);
        }
    }



    void OnGamePause()
    {
        foreach (BulletController bullet in _activeBullets)
            bullet._rigidbody.linearVelocity = Vector2.zero;
    }

    void OnGameUnpause()
    {
        foreach (BulletController bullet in _activeBullets)
            bullet._rigidbody.linearVelocity = bullet.transform.up * _bulletSpeed;
    }



    void BulletHitEnemy(BulletController bullet, Collider2D enemyCollider)
    {
        if (enemyCollider.TryGetComponent(out Melee_EnemyController enemy))
        {
            Melee_EnemyManager.Instance.DamageEnemy(enemy, _baseStats.BaseDamage);

            _bulletPool.Release(bullet);
        }
    }

    public void OnPauseableFixedUpdate(float deltaTime)
    {
        foreach (BulletController bullet in _activeBullets)
            bullet._rigidbody.linearVelocity = transform.up * _bulletSpeed;
    }



    void Shoot()
    {
        _currentAmmo -= 1;      // TO BE CHANGED: MULTI-SHOT
        _lastShotTime = Time.time;

        if (_currentAmmo <= 0) 
            _reloadCoroutine = StartCoroutine(Reload());

        _bulletPool.Get();
    }



    IEnumerator Reload()
    {
        _isReloading = true;

        float elapsedTime = 0f;
        while (elapsedTime < _reloadTime)
        {
            if (!PauseManager.isPaused)
                elapsedTime += Time.deltaTime;

            yield return null;
        }

        _currentAmmo = _maxAmmo;

        _isReloading = false;
        StopCoroutine(_reloadCoroutine);
    }



    #region _bulletPool
    BulletController CreateBullet()
    {
        BulletController newBullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation, _bulletPoolParent);
        newBullet.gameObject.SetActive(false);
        newBullet.name = "Pooled Bullet";
        return newBullet;
    }

    void GetBullet(BulletController bullet)
    {
        bullet._spawnTime = Time.time;
        _activeBullets.Add(bullet);

        bullet.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
        bullet.gameObject.SetActive(true);
        bullet._rigidbody.linearVelocity = bullet.transform.up * _bulletSpeed;
    }

    void ReleaseBullet(BulletController bullet)
    {
        _activeBullets.Remove(bullet);

        bullet._rigidbody.linearVelocity = Vector2.zero;

        bullet.gameObject.SetActive(false); 
    }

    void DestroyBullet(BulletController bullet)
    {
        Destroy(bullet);
    }
    #endregion



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);

        PauseManager.OnGamePause -= OnGamePause;
        PauseManager.OnGameUnpause -= OnGameUnpause;
        BulletController.OnBulletCollidesEnemy -= BulletHitEnemy;
    }
}