using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IPauseableUpdate, IPauseableFixedUpdate
{
    [SerializeField] private PlayerBaseStats _baseStats;
    private float _speed;
    private float _rotation;
    private InputAction _movementAction; 
    private Vector2 _movementDirection; 
    private Rigidbody2D _rigidBody; 



    void Awake()
    {
        _speed = _baseStats.BaseMoveSpeed;
    }

    void OnEnable()
    {
        UpdateManager.Instance.RegisterForUpdate(this);
        UpdateManager.Instance.RegisterForFixedUpdate(this);

        PauseManager.OnGamePause += OnGamePause;
    }



    void Start()
    {
        // Gather components 
        _movementAction = InputSystem.actions.FindAction("Move");
        _rigidBody = GetComponent<Rigidbody2D>();
    }



    public void OnPauseableUpdate(float deltaTime)
    {
        // Calculate movement vector based on wasd 
        Vector2 moveValue = _movementAction.ReadValue<Vector2>();  
        _movementDirection = new Vector2(moveValue.x, moveValue.y);
        

        // Calculate rotation based on mouse position 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lookDir = mousePos - _rigidBody.position;
        _rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    }



    public void OnPauseableFixedUpdate(float deltaTime)
    {
        _rigidBody.linearVelocity = _speed * _movementDirection;
        _rigidBody.rotation = _rotation;
    }



    void OnGamePause()
    {
        _rigidBody.linearVelocity = Vector2.zero;
    }



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);
        UpdateManager.Instance.UnregisterFromFixedUpdate(this);

        PauseManager.OnGamePause -= OnGamePause;
    }
}