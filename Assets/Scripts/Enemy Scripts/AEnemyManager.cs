using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class AEnemyManager : MonoBehaviour, IPauseableUpdate, IPauseableFixedUpdate
{
    [Header("Setup")]
    [SerializeField] protected Transform _playerTransform;
    [SerializeField] protected StrawberryController _enemyPrefab;
    [SerializeField] protected MeleeEnemyBaseStats _enemyBaseStats;

    [Header("Spawn timing")]
    [SerializeField] protected float _minSpawnTime = 0.1f;
    [SerializeField] protected float _maxSpawnTime = 2.0f;
    [SerializeField] protected float _nextSpawnTime = 0f;

    [Header("Pool details")]
    [SerializeField] protected int _poolDefaultCapacity = 120;
    [SerializeField] protected int _poolDefaultSize = 1080;
    [SerializeField] protected bool _collectionCheck = true;

    protected PauseService _pauseService;
    
    protected float _randomSize;

    protected Vector2 _faceDirection;
    protected float _rotation;

    protected ObjectPool<IEnemy> _enemyPool;
    protected readonly List<IEnemy> _activeEnemies = new();

    

    void Awake()
    {
        _pauseService = ServiceLocator.TryGet<PauseService>();
            
        _enemyPool = new ObjectPool<IEnemy>(
            createFunc: CreateEnemy,
            actionOnGet: GetEnemy,
            actionOnRelease: ReleaseEnemy,
            actionOnDestroy: DestroyEnemy,
            collectionCheck: _collectionCheck,   
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolDefaultSize
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

        Initialize();
    }

    public abstract void Initialize();



    public virtual void OnPauseableUpdate(float deltaTime)
    {
        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + Random.Range(_minSpawnTime, _maxSpawnTime);

            _enemyPool.Get();
        }
    }

    public virtual void OnPauseableFixedUpdate(float deltaTime)
    {
        foreach (IEnemy enemy in _activeEnemies)
        {
            RotateAndMoveEnemyTowardPlayer(enemy);
        }
    }

    public virtual void OnGamePause()
    {
        foreach (IEnemy enemy in _activeEnemies)
            enemy.EnemyRigidbody.linearVelocity = Vector2.zero;
    }



    void RotateAndMoveEnemyTowardPlayer(IEnemy enemy)
    {
        _faceDirection = (Vector2) _playerTransform.position - enemy.EnemyRigidbody.position;
        _rotation = Mathf.Atan2(_faceDirection.y, _faceDirection.x) * Mathf.Rad2Deg - 90f;

        enemy.EnemyRigidbody.rotation = _rotation;
        enemy.SetMovement(_enemyBaseStats.BaseMoveSpeed);
    }



    public abstract Vector2 RandomLocation();

    float RandomSize()
    {
        _randomSize = Random.Range(_enemyBaseStats.MinSize, _enemyBaseStats.MaxSize);

        return _randomSize;
    }



    #region _enemyPool
    public virtual IEnemy CreateEnemy()
    {
        IEnemy newEnemy = Instantiate(_enemyPrefab);
        newEnemy.Reset();
        newEnemy.OnDeath += (e) => _enemyPool.Release(e);
        return newEnemy;
    }

    public virtual void GetEnemy(IEnemy enemy)
    {
        _activeEnemies.Add(enemy);

        enemy.Spawn(RandomLocation(), 0f, transform, _enemyBaseStats.BaseHealth, RandomSize());
    }

    public virtual void ReleaseEnemy(IEnemy enemy)
    {
        _activeEnemies.Remove(enemy);

        enemy.Reset();
    }

    public virtual void DestroyEnemy(IEnemy enemy)
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
