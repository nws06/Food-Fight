using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    private bool _isReloading;
    private float _lastShotTime;
    private InputAction _shootAction;
    private InputAction _reloadAction;
    private Coroutine _reloadCoroutine;



    void Awake()
    {
        _damage = _baseStats.BaseDamage;
        _maxAmmo = _baseStats.BaseMaxAmmo;
        _reloadTime = _baseStats.BaseReloadTime;
        _bulletSpeed = _baseStats.BaseBulletSpeed;
        _shotCooldown = _baseStats.BaseShotCooldown;

        _currentAmmo = _maxAmmo;

        _lastShotTime = -5f;

        _shootAction = InputSystem.actions.FindAction("Attack");
        _reloadAction = InputSystem.actions.FindAction("Reload");
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

        print("SHOOT");
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
}
