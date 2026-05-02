using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float baseSpeed;
    private float speed;
    private float rotation;
    private Vector2 movement; 
    private InputAction moveAction; 
    private Rigidbody2D rb; 



    void Awake()
    {
        // Set values
        baseSpeed = 25f;
        speed = baseSpeed;
    }



    void Start()
    {
        // Gather components 
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        // Calculate movement vector based on wasd 
        Vector2 moveValue = moveAction.ReadValue<Vector2>();  
        movement = new Vector2(moveValue.x, moveValue.y);
        

        // Calculate rotation based on mouse position 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lookDir = mousePos - rb.position;
        rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    }



    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
        rb.rotation = rotation;
    }
}