using System;
using UnityEngine;

[System.Serializable]
public class PauseService
{
    public event Action OnGamePause;
    public event Action OnGameUnpause;

    [field: SerializeField] public bool IsPaused { get; private set; }



    public PauseService()
    {
        IsPaused = false;
    }



    public void TogglePause()
    {
        if (IsPaused)
            UnpauseGame();
        else 
            PauseGame();
    }



    void PauseGame()
    {
        IsPaused = true;
        OnGamePause?.Invoke();
    }

    void UnpauseGame()
    {
        IsPaused = false;       
        OnGameUnpause?.Invoke();
    }

    public void ForceUnpause()
    {
        if (IsPaused)
            UnpauseGame();
    }
}