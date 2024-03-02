using System;
using Unity.Netcode;

namespace com.github.zehsteam.CoilHeadSettings;

[Serializable]
internal class SyncedConfigData : INetworkSerializable
{
    public int powerLevel;
    public int maxSpawned;
    public int attackDamage;
    public float attackSpeed;
    public float movementSpeed;

    public SyncedConfigData() { }

    public SyncedConfigData(SyncedConfig configManager)
    {
        powerLevel = configManager.PowerLevel;
        maxSpawned = configManager.MaxSpawned;
        attackDamage = configManager.AttackDamage;
        attackSpeed = configManager.AttackSpeed;
        movementSpeed = configManager.MovementSpeed;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref powerLevel);
        serializer.SerializeValue(ref maxSpawned);
        serializer.SerializeValue(ref attackDamage);
        serializer.SerializeValue(ref attackSpeed);
        serializer.SerializeValue(ref movementSpeed);
    }
}
