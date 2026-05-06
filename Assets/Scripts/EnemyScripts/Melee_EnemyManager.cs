using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Melee_EnemyManager : MonoBehaviour, IPauseableUpdate, IPauseableFixedUpdate
{
    public static Melee_EnemyManager Instance;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Melee_EnemyController _enemyPrefab;
    [SerializeField] private Melee_EnemyBaseStats _enemyBaseStats;
    [SerializeField] private PlayerBaseStats _playerBaseStats;
    [SerializeField] private float _minSpawnTime = 0.1f;
    [SerializeField] private float _maxSpawnTime = 2.0f;

    private float _nextSpawnTime = 0f;
    private Vector3 _spawnPosition = new Vector3(0f, 0f, -10f);

    private float _randomXPosition;
    private float _randomYPosition;
    private Vector3 _randomPosition;
    private float _randomSize;

    private Vector2 _faceDirection;

    private ObjectPool<Melee_EnemyController> _enemyPool;
    private List<Melee_EnemyController> _activeEnemies = new List<Melee_EnemyController>();



    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);


            
        _enemyPool = new ObjectPool<Melee_EnemyController>(
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
        PauseManager.OnGamePause += OnGamePause;
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
        foreach (Melee_EnemyController enemy in _activeEnemies)
        {
            RotateEnemy(enemy);
            MoveEnemy(enemy);
        }
    }



    void RotateEnemy(Melee_EnemyController enemy)
    {
        _faceDirection = (Vector2)_playerTransform.position - enemy._rigidbody.position;
        enemy._rigidbody.rotation = Mathf.Atan2(_faceDirection.y, _faceDirection.x) * Mathf.Rad2Deg - 90f;
    }

    void MoveEnemy(Melee_EnemyController enemy)
    {
        enemy._rigidbody.linearVelocity = enemy._rigidbody.transform.up * _enemyBaseStats.BaseMoveSpeed;
    }



    public void DamageEnemy(Melee_EnemyController enemy, float damage)
    {
        enemy._currentHp -= damage;

        if (enemy._currentHp <= 0)
            KillEnemy(enemy);
    }

    void KillEnemy(Melee_EnemyController enemy)
    {
        // Death animation
        // Spawn XP

        _enemyPool.Release(enemy);
    }



    void OnGamePause()
    {
        foreach (Melee_EnemyController enemy in _activeEnemies)
            enemy._rigidbody.linearVelocity = Vector2.zero;
    }



    /// <summary>
    /// Set the given enemy's position to a random vector that is, relative to the player's transform: <br />
    /// between 80 and 120 units away on the x axis <br />
    /// between 25 and 75 units away on the y axis 
    /// </summary>
    void RandomizeLocation(GameObject enemy)
    {
        // X position: 
        _randomXPosition = (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, 30f);
        _randomXPosition = (_randomXPosition >= 0) ? _randomXPosition + 80 : _randomXPosition - 80;
        _randomXPosition += _playerTransform.position.x;

        // Y position: 
        _randomYPosition = (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, 25f);
        _randomYPosition = (_randomYPosition >= 0) ? _randomYPosition + 50 : _randomYPosition - 50;
        _randomYPosition += _playerTransform.position.y;

        _randomPosition = new Vector3(_randomXPosition, _randomYPosition, 0f);

        enemy.transform.position = _randomPosition;
    }



    void RandomizeSize(GameObject enemy)
    {
        _randomSize = Random.Range(_enemyBaseStats.MinSize, _enemyBaseStats.MaxSize);
        enemy.transform.localScale = new Vector3(_randomSize, _randomSize, _randomSize);
    }



    #region _enemyPool
    Melee_EnemyController CreateEnemy()
    {
        Melee_EnemyController newEnemy = Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity, transform);
        newEnemy.gameObject.SetActive(false);
        newEnemy.name = "Pooled Melee Strawberry";
        return newEnemy;
    }

    void GetEnemy(Melee_EnemyController enemy)
    {
        _activeEnemies.Add(enemy);

        RandomizeLocation(enemy.gameObject);
        RandomizeSize(enemy.gameObject);

        enemy._currentHp = _enemyBaseStats.BaseHealth;

        enemy.gameObject.SetActive(true);
    }

    void ReleaseEnemy(Melee_EnemyController enemy)
    {
        _activeEnemies.Remove(enemy);

        enemy._currentHp = 0;

        enemy.gameObject.SetActive(false);
    }

    void DestroyEnemy(Melee_EnemyController enemy)
    {
        Destroy(enemy);
    }
    #endregion



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);
        UpdateManager.Instance.UnregisterFromFixedUpdate(this);

        PauseManager.OnGamePause -= OnGamePause;
    }
}
