using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour, IPauseableUpdate
{
    private PlayerStatManager _playerStats;
    private PauseService _pauseService;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private BulletController _bulletPrefab;
    private ObjectPool<BulletController> _bulletPool;
    private List<BulletController> _activeBullets = new List<BulletController>();



    void Awake()
    {
        _playerStats = ServiceLocator.Get<PlayerStatManager>();
        _pauseService = ServiceLocator.TryGet<PauseService>();

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

        _pauseService.OnGamePause += OnGamePause;
        _pauseService.OnGameUnpause += OnGameUnpause;
        BulletController.OnBulletCollidesEnemy += BulletHitEnemy;
    }



    public void OnPauseableUpdate(float deltaTime)
    {
        for (int i = _activeBullets.Count - 1; i >= 0; i--)
        {
            _activeBullets[i]._lifetime += deltaTime;

            if (_activeBullets[i]._lifetime >= _playerStats.CurrentStats.BulletLifetime)
                _bulletPool.Release(_activeBullets[i]);
        }
    }



    public void Shoot()
    {
        _bulletPool.Get();
    }



    void OnGamePause()
    {
        foreach (BulletController bullet in _activeBullets)
            bullet.StopMovement();
    }

    void OnGameUnpause()
    {
        foreach (BulletController bullet in _activeBullets)
            bullet.StartMovement(_playerStats.CurrentStats.BulletSpeed);
    }



    void BulletHitEnemy(BulletController bullet, Collider2D enemyCollider)
    {
        if (enemyCollider.TryGetComponent(out Melee_EnemyController enemy))
        {
            Melee_EnemyManager.Instance.DamageEnemy(enemy, _playerStats.CurrentStats.Damage);

            _bulletPool.Release(bullet);
        }
    }



    #region _bulletPool
    BulletController CreateBullet()
    {
        BulletController newBullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation, transform);
        newBullet.gameObject.SetActive(false);
        newBullet.name = "Pooled Bullet";
        return newBullet;
    }

    void GetBullet(BulletController bullet)
    {
        _activeBullets.Add(bullet);

        bullet.Initialize(_firePoint.position, _firePoint.rotation, _playerStats.CurrentStats.BulletSpeed);
    }

    void ReleaseBullet(BulletController bullet)
    {
        _activeBullets.Remove(bullet);

        bullet.Terminate();
    }

    void DestroyBullet(BulletController bullet)
    {
        Destroy(bullet);
    }
    #endregion



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);

        _pauseService.OnGamePause -= OnGamePause;
        _pauseService.OnGameUnpause -= OnGameUnpause;
        BulletController.OnBulletCollidesEnemy -= BulletHitEnemy;
    }
}