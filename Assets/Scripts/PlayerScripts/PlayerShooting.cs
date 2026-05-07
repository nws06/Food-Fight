using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShooting : MonoBehaviour, IPauseableUpdate
{
    [SerializeField] private PlayerBaseStats _baseStats;
    [SerializeField] private BulletManager _bulletManager;
    private bool _isReloading;
    private int _currentAmmo;
    private float _lastShotTime = 0f;
    private InputAction _shootAction;
    private InputAction _reloadAction;
    private Coroutine _reloadCoroutine;



    void Awake()
    {
        _shootAction = InputSystem.actions.FindAction("Attack");
        _reloadAction = InputSystem.actions.FindAction("Reload");
    }

    void Start()
    {
        UpdateManager.Instance.RegisterForUpdate(this);
    }



    public void OnPauseableUpdate(float deltaTime)
    {
        if (_shootAction.IsPressed() && !_isReloading && Utils.IsOffCooldown(_lastShotTime, _baseStats.BaseShotCooldown))
        {
            if (_currentAmmo > 0)
                Shoot();
            else if (!_isReloading)
                _reloadCoroutine = StartCoroutine(Reload()); 
        }

        if (_reloadAction.IsPressed() && !_isReloading && _currentAmmo < _baseStats.BaseMaxAmmo)
            _reloadCoroutine = StartCoroutine(Reload()); 
    }



    void Shoot()
    {
        _currentAmmo -= 1;      // TO BE CHANGED: MULTI-SHOT
        _lastShotTime = Time.time;

        if (_currentAmmo <= 0) 
            _reloadCoroutine = StartCoroutine(Reload());

        _bulletManager.Shoot();
    }



    IEnumerator Reload()
    {
        _isReloading = true;

        float elapsedTime = 0f;
        while (elapsedTime < _baseStats.BaseReloadTime)
        {
            if (!PauseManager.isPaused)
                elapsedTime += Time.deltaTime;

            yield return null;
        }

        _currentAmmo = _baseStats.BaseMaxAmmo;

        _isReloading = false;
        StopCoroutine(_reloadCoroutine);
    }



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);
    }
}