// Needs to have the earliest DefaultExecutionOrder.
// Needs to be the only class handling registration of services.

// HOW TO USE: 
// In the class that depends on a given service, store a reference to 
// the desired service using ServiceLocator.Get<DesiredType>().
// You can then access methods/properties/etc from that reference.



using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ServiceLocatorInitializer : MonoBehaviour
{
    [Header("PlayerStatManager")]
    [SerializeField] private PlayerBaseStats _baseStats;
    [SerializeField] private UpgradeData[] _allUpgradeData;

    [Header("DEBUG DETAILS -- DO NOT ASSIGN IN INSPECTOR")]
    [SerializeField] private PlayerStatManager _playerStatManager;



    void Awake()
    {
        DontDestroyOnLoad(this);

        _playerStatManager = new PlayerStatManager(_baseStats, _allUpgradeData);
        ServiceLocator.Register(_playerStatManager);
    }
}
