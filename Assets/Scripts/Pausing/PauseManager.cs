using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private PauseService _pauseService;
    private InputAction _pauseAction;
    private float _pauseCooldown = 0.5f;
    private float _lastPauseTime = -1f;



    void Awake()
    {
        _pauseService = ServiceLocator.TryGet<PauseService>();

        _pauseAction = InputSystem.actions.FindAction("Escape");
    }



    void Update()
    {
        if (_pauseAction.IsPressed() && Utils.IsOffCooldown(_lastPauseTime, _pauseCooldown))
        {
            _pauseService.TogglePause();
            _lastPauseTime = Time.time;
        }
    }



    void OnDestroy()
    {
        _pauseService.ForceUnpause();
    }
}
