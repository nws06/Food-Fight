using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Melee_EnemyManager : MonoBehaviour, IPauseableUpdate, IPauseableFixedUpdate
{
    public Melee_EnemyController _strawberryPrefab;
    public Melee_EnemyController _spawningEnemy;
    public Transform _playerTransform;
    public ObjectPool<Melee_EnemyController> _enemyPool;
    public Vector2 _spawnPosition = new Vector2(-50f, 0f);
    public Quaternion _rotation = Quaternion.identity;

    [SerializeField] private Melee_EnemyBaseStats _strawberryBaseStats;
    [SerializeField] private float _minSpawnTime = 0.1f;
    [SerializeField] private float _maxSpawnTime = 2.0f;
    private float _nextSpawnTime = 0f;
    private float _randomXPosition;
    private float _randomYPosition;
    private Vector3 _randomPosition;
    private List<GameObject> _activeEnemies = new List<GameObject>();



    void Awake()
    {
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



    void Start()
    {
        UpdateManager.Instance.RegisterForUpdate(this);
        UpdateManager.Instance.RegisterForFixedUpdate(this);

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }



    void SpawnEnemy()
    {
        _enemyPool.Get();
    }



    public void OnPauseableUpdate(float deltaTime)
    {
        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + Random.Range(_minSpawnTime, _maxSpawnTime);

            SpawnEnemy();
        }
    }

    public void OnPauseableFixedUpdate(float deltaTime)
    {
        
    }



    /// <summary>
    /// Set the given enemy's position to a random vector that is, relative to the player's transform: <br />
    /// between 80 and 120 units away on the x axis <br />
    /// between 25 and 75 units away on the y axis 
    /// </summary>
    public void RandomizeLocation(GameObject enemy)
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



    #region _enemyPool
    Melee_EnemyController CreateEnemy()
    {
        Melee_EnemyController newEnemy = Instantiate(_strawberryPrefab, _spawnPosition, _rotation, transform);
        newEnemy.gameObject.SetActive(false);
        newEnemy.name = "Pooled Melee Strawberry";
        return newEnemy;
    }

    void GetEnemy(Melee_EnemyController enemy)
    {
        _activeEnemies.Add(enemy.gameObject);

        RandomizeLocation(enemy.gameObject);

        enemy.gameObject.SetActive(true);
    }

    void ReleaseEnemy(Melee_EnemyController enemy)
    {
        _activeEnemies.Remove(enemy.gameObject);

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
    }
}
