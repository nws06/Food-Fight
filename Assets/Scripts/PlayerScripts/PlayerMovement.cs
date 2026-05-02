using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private float _baseSpeed;
    private float _speed;
    private float _rotation;
    private InputAction _movementAction; 
    private Vector2 _movementDirection; 
    private Rigidbody2D _rigidBody; 



    void Awake()
    {
        // Set values
        _baseSpeed = 25f;
        _speed = _baseSpeed;
    }



    void Start()
    {
        // Gather components 
        _movementAction = InputSystem.actions.FindAction("Move");
        _rigidBody = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        // Calculate movement vector based on wasd 
        Vector2 moveValue = _movementAction.ReadValue<Vector2>();  
        _movementDirection = new Vector2(moveValue.x, moveValue.y);
        

        // Calculate rotation based on mouse position 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lookDir = mousePos - _rigidBody.position;
        _rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    }



    void FixedUpdate()
    {
        _rigidBody.linearVelocity = _movementDirection * _speed;
        _rigidBody.rotation = _rotation;
    }
}