using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeConfig")]
public class UpgradeData : ScriptableObject
{
    [field: SerializeField]
    public UpgradeType Type { get; private set; }

    [field: SerializeField]
    public float Value { get; private set; }
    [field: SerializeField]
    public int MaxOwned { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public string Description { get; private set; }
    [field: SerializeField]
    public string Details { get; private set; }
}
