using System;
using UnityEngine;

[System.Serializable]
public class PlayerStatManager 
{
    public RuntimePlayerStats CurrentStats { get => _currentStats; }

    [SerializeField] private RuntimePlayerStats _currentStats;
    [SerializeField] private Upgrade[] _allUpgrades;

    public PlayerStatManager(PlayerBaseStats baseStats, UpgradeData[] allUpgradeData)
    {
        _currentStats = new RuntimePlayerStats(baseStats);

        _allUpgrades = new Upgrade[allUpgradeData.Length];

        for (int i = 0; i < allUpgradeData.Length; i++) {
            _allUpgrades[i] = new Upgrade(allUpgradeData[i]);
        }

        SortUpgrades();
    }



    public void ApplyUpgrade(UpgradeType upgradeType)
    {
        _currentStats.ApplyUpgrade(_allUpgrades[(int) upgradeType]);
    }



    void SortUpgrades()
    {
        // Sort based on UpgradeType enum
        Array.Sort(_allUpgrades, (a, b) => a.Data.Type.CompareTo(b.Data.Type));
    }
}
