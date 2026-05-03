using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    private List<IPauseableUpdate> _updateList = new List<IPauseableUpdate>();
    private List<IPauseableFixedUpdate> _fixedUpdateList = new List<IPauseableFixedUpdate>();
    private float deltaTime;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    void Update()
    {
        deltaTime = Time.deltaTime;

        if (PauseManager.Instance == null || !PauseManager.isPaused)
        {
            foreach (IPauseableUpdate item in _updateList) 
                item.OnPauseableUpdate(deltaTime);
        }
    }

    void FixedUpdate()
    {
        deltaTime = Time.deltaTime;

        if (PauseManager.Instance == null || !PauseManager.isPaused)
        {
            foreach (IPauseableFixedUpdate item in _fixedUpdateList) 
                item.OnPauseableFixedUpdate(deltaTime);
        }
    }



    public void RegisterForUpdate(IPauseableUpdate item)
    {
        if (!_updateList.Contains(item)) 
            _updateList.Add(item);
    }

    public void UnregisterFromUpdate(IPauseableUpdate item)
    {
        _updateList.Remove(item);
    }



    public void RegisterForFixedUpdate(IPauseableFixedUpdate item)
    {
        if (!_fixedUpdateList.Contains(item)) 
            _fixedUpdateList.Add(item);
    }

    public void UnregisterFromFixedUpdate(IPauseableFixedUpdate item)
    {
        _fixedUpdateList.Remove(item);
    }
}
