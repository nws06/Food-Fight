using System;
using UnityEngine;

public class DEBUG_PlayerStatsTesting : MonoBehaviour
{
    private PlayerStatManager psm;

    void Start()
    {
        psm = ServiceLocator.Get<PlayerStatManager>();

        psm.ApplyUpgrade(UpgradeType.MoveSpeed);
    }
}
