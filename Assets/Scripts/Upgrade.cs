using System;
using UnityEngine;

public enum UpgradeType
{
    DefaultUpgradeType,
    MoveSpeed,
    MaxHealth,

    Damage,
    ReloadTime,
    ShotCooldown,
    BulletSpeed,
    MaxAmmo,
    BulletLifetime
}



[System.Serializable]
public class Upgrade 
{
    [field: SerializeField]
    public UpgradeData Data { get; private set; }
    [field: SerializeField]
    public int CurrentOwned { get; private set; }

    public float TotalValue { get => Data.Value * CurrentOwned; }

    public string FormattedDescription { get => string.Format(Data.Description, Data.Value * 100); }
    public string FormattedDetails { get => string.Format(Data.Details, TotalValue * 100); }



    public Upgrade(UpgradeData data)
    {
        Data = data;
        CurrentOwned = 0;
    }



    public bool TryAcquire()
    {
        if (CurrentOwned < Data.MaxOwned)
        {
            CurrentOwned++;
            return true;
        }
        else
        {
            Debug.LogWarning($"Upgrade.cs: Acquiring another copy of {this.Data.Name} exceeds the max available.\n" + 
                             $"You have {this.CurrentOwned} and the max is {this.Data.MaxOwned}.");
            return false;
        }
    }
}