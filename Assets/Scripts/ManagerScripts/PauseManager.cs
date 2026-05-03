using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private bool isPaused;



    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        isPaused = false;
    }



    void OnDestroy()
    {
        // Clear instance and unpause when destroyed (scene change)
        Instance = null;
        isPaused = false;
    }
}
