using UnityEngine;

public class Melee_EnemyController : MonoBehaviour, IPauseableFixedUpdate
{
    void OnEnable()
    {
        UpdateManager.Instance.RegisterForFixedUpdate(this);
    }



    public void OnPauseableFixedUpdate(float deltaTime)
    {
        
    }



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromFixedUpdate(this);
    }
}
