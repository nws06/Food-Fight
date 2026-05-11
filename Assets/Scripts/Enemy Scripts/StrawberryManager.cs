using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StrawberryManager : MonoBehaviour, IPauseableUpdate, IPauseableFixedUpdate
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private StrawberryController _enemyPrefab;
    [SerializeField] private MeleeEnemyBaseStats _enemyBaseStats;
    [SerializeField] private PlayerBaseStats _playerBaseStats;
    [SerializeField] private float _minSpawnTime = 0.1f;
    [SerializeField] private float _maxSpawnTime = 2.0f;
    private PauseService _pauseService;

    private float _nextSpawnTime = 0f;
    private Vector3 _spawnPosition = new Vector3(0f, 0f, -10f);

    private float _randomXPosition;
    private float _randomYPosition;
    private Vector2 _randomPosition;
    private float _randomSize;

    private Vector2 _faceDirection;
    private float _rotation;

    private ObjectPool<IEnemy> _enemyPool;
    private List<IEnemy> _activeEnemies = new List<IEnemy>();



    void Awake()
    {
        _pauseService = ServiceLocator.TryGet<PauseService>();
            
        _enemyPool = new ObjectPool<IEnemy>(
            createFunc: CreateEnemy,
            actionOnGet: GetEnemy,
            actionOnRelease: ReleaseEnemy,
            actionOnDestroy: DestroyEnemy,
            collectionCheck: true,   
            defaultCapacity: 5,
            maxSize: 10
        );
    }

    void OnEnable()
    {
        _pauseService.OnGamePause += OnGamePause;
    }

    void Start()
    {
        UpdateManager.Instance.RegisterForUpdate(this);
        UpdateManager.Instance.RegisterForFixedUpdate(this);

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }



    public void OnPauseableUpdate(float deltaTime)
    {
        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + Random.Range(_minSpawnTime, _maxSpawnTime);

            _enemyPool.Get();
        }
    }

    public void OnPauseableFixedUpdate(float deltaTime)
    {
        foreach (IEnemy enemy in _activeEnemies)
        {
            RotateAndMoveEnemyTowardPlayer(enemy);
        }
    }



    void RotateAndMoveEnemyTowardPlayer(IEnemy enemy)
    {
        _faceDirection = (Vector2) _playerTransform.position - enemy.EnemyRigidbody.position;
        _rotation = Mathf.Atan2(_faceDirection.y, _faceDirection.x) * Mathf.Rad2Deg - 90f;

        enemy.EnemyRigidbody.rotation = _rotation;
        enemy.SetMovement(_enemyBaseStats.BaseMoveSpeed);
    }



    /*void RotateEnemy(IEnemy enemy)
    {
        _faceDirection = (Vector2)_playerTransform.position - enemy.EnemyRigidbody.position;
        enemy.EnemyRigidbody.rotation = Mathf.Atan2(_faceDirection.y, _faceDirection.x) * Mathf.Rad2Deg - 90f;
    }

    void MoveEnemy(IEnemy enemy)
    {
        enemy.EnemyRigidbody.linearVelocity = enemy.EnemyRigidbody.transform.up * _enemyBaseStats.BaseMoveSpeed;
    }



    public void DamageEnemy(IEnemy enemy, float damage)
    {
        enemy.Damage(damage);
    }

    void KillEnemy(IEnemy enemy)
    {
        // Death animation
        // Spawn XP

        _enemyPool.Release(enemy);
    }*/



    void OnGamePause()
    {
        foreach (IEnemy enemy in _activeEnemies)
            enemy.EnemyRigidbody.linearVelocity = Vector2.zero;
    }



    /// <summary>
    /// Set the given enemy's position to a random vector that is, relative to the player's transform: <br />
    /// between 80 and 120 units away on the x axis <br />
    /// between 25 and 75 units away on the y axis 
    /// </summary>
    Vector2 RandomLocation()
    {
        // X position: 
        _randomXPosition = (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, 30f);
        _randomXPosition = (_randomXPosition >= 0) ? _randomXPosition + 80 : _randomXPosition - 80;
        _randomXPosition += _playerTransform.position.x;

        // Y position: 
        _randomYPosition = (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, 25f);
        _randomYPosition = (_randomYPosition >= 0) ? _randomYPosition + 50 : _randomYPosition - 50;
        _randomYPosition += _playerTransform.position.y;

        _randomPosition = new Vector2(_randomXPosition, _randomYPosition);

        print(_randomPosition);
        return _randomPosition;
    }



    float RandomSize()
    {
        _randomSize = Random.Range(_enemyBaseStats.MinSize, _enemyBaseStats.MaxSize);

        return _randomSize;
    }



    // FIX THIS STUFF UPP!!!
    // MAKE RESETTING CLEANER ETC ETC
    #region _enemyPool
    IEnemy CreateEnemy()
    {
        IEnemy newEnemy = Instantiate(_enemyPrefab);
        newEnemy.Reset();
        newEnemy.OnDeath += (e) => _enemyPool.Release(e);
        return newEnemy;
    }

    void GetEnemy(IEnemy enemy)
    {
        _activeEnemies.Add(enemy);

        enemy.Spawn(RandomLocation(), 0f, transform, _enemyBaseStats.BaseHealth, RandomSize());
    }

    void ReleaseEnemy(IEnemy enemy)
    {
        _activeEnemies.Remove(enemy);

        enemy.Reset();
    }

    void DestroyEnemy(IEnemy enemy)
    {
        enemy.OnDeath = null;
        enemy.Destroy();
    }
    #endregion



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);
        UpdateManager.Instance.UnregisterFromFixedUpdate(this);

        _pauseService.OnGamePause -= OnGamePause;
    }
}
