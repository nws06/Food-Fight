using System;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;

    public RuntimePlayerStats CurrentStats { get => _currentStats; }

    [SerializeField] private RuntimePlayerStats _currentStats;
    [SerializeField] private Upgrade[] _allUpgrades;

    [Header("Assign these in inspector")]
    [SerializeField] private PlayerBaseStats _baseStats;        // Assigned in inspector
    [SerializeField] private UpgradeData[] _allUpgradeData;     // Assigned in inspector

    

    void Awake()
    {
        // Make singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
            return;
        }



        _currentStats = new RuntimePlayerStats(_baseStats);

        _allUpgrades = new Upgrade[_allUpgradeData.Length];

        for (int i = 0; i < _allUpgradeData.Length; i++) {
            _allUpgrades[i] = new Upgrade(_allUpgradeData[i]);
        }

        SortUpgrades();
    }



    public void ApplyUpgrade(Upgrade upgrade)
    {
        _currentStats.ApplyUpgrade(upgrade);
    }



    void SortUpgrades()
    {
        // Sort based on UpgradeType enum
        Array.Sort(_allUpgrades, (a, b) => a.Data.Type.CompareTo(b.Data.Type));
    }
}
