using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public static event Action OnGamePause;
    public static event Action OnGameUnpause;
    public static bool isPaused = false;

    private InputAction _pauseAction;
    private float _pauseCooldown = 0.5f;
    private float _lastPauseTime = -1f;



    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _pauseAction = InputSystem.actions.FindAction("Escape");
    }



    void Update()
    {
        if (_pauseAction.IsPressed() && Utils.IsOffCooldown(_lastPauseTime, _pauseCooldown))
        {
            if (!isPaused)
                PauseGame();
            else
                UnpauseGame();

                _lastPauseTime = Time.time;
        }
    }



    void PauseGame()
    {
        isPaused = true;
        print("Pause");

        OnGamePause?.Invoke();
    }

    void UnpauseGame()
    {
        isPaused = false;
        print("Unpause");
        
        OnGameUnpause?.Invoke();
    }



    void OnDestroy()
    {
        // Clear instance and unpause when destroyed (scene change)
        Instance = null;
        isPaused = false;
    }
}
