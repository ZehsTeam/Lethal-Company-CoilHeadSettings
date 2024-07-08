using System;
using Unity.Netcode;

namespace com.github.zehsteam.CoilHeadSettings;

[Serializable]
internal class SyncedConfigData : INetworkSerializable
{
    // Coil-Head Settings
    public int AttackDamage;
    public float AttackSpeed;
    public float MovementSpeed;

    public SyncedConfigData() { }

    public SyncedConfigData(SyncedConfigManager configManager)
    {
        // Coil-Head Settings
        AttackDamage = configManager.AttackDamage.Value;
        AttackSpeed = configManager.AttackSpeed.Value;
        MovementSpeed = configManager.MovementSpeed.Value;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // Coil-Head Settings
        serializer.SerializeValue(ref AttackDamage);
        serializer.SerializeValue(ref AttackSpeed);
        serializer.SerializeValue(ref MovementSpeed);
    }
}
