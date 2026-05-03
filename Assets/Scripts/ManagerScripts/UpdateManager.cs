using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    private List<IPauseableUpdate> _updateList = new List<IPauseableUpdate>();
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



    public void Register(IPauseableUpdate item)
    {
        if (!_updateList.Contains(item)) 
            _updateList.Add(item);
    }

    public void Unregister(IPauseableUpdate item)
    {
        _updateList.Remove(item);
    }
}
