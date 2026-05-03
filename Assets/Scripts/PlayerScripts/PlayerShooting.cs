using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerBaseStats _baseStats;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
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
    private ObjectPool<GameObject> _bulletPool;



    void Awake()
    {
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

        _bulletPool = new ObjectPool<GameObject>(
            createFunc: CreateBullet,
            actionOnGet: GetBullet,
            actionOnRelease: ReleaseBullet,
            actionOnDestroy: DestroyBullet,
            collectionCheck: true,   
            defaultCapacity: 5,
            maxSize: 10
        );
    }



    void Update()
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
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _currentAmmo = _maxAmmo;

        _isReloading = false;
        StopCoroutine(_reloadCoroutine);
    }



    GameObject CreateBullet()
    {
        GameObject newBullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation, transform);
        newBullet.SetActive(false);
        return newBullet;
    }

    void GetBullet(GameObject bullet)
    {
        bullet.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
        bullet.SetActive(true);

        StartCoroutine(BulletLifetime(bullet));
    }

    void ReleaseBullet(GameObject bullet)
    {
        bullet.SetActive(false); 
    }

    void DestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }



    IEnumerator BulletLifetime(GameObject bullet)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _bulletLifetime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        ReleaseBullet(bullet);
    }
}