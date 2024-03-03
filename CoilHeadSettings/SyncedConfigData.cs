using System;
using Unity.Netcode;

namespace com.github.zehsteam.CoilHeadSettings;

[Serializable]
internal class SyncedConfigData : INetworkSerializable
{
    // Settings
    public int powerLevel;
    public int attackDamage;
    public float attackSpeed;
    public float movementSpeed;

    // Spawn Settings
    public int maxSpawned;
    public float spawnWeightMultiplier;
    public int offenseSpawnWeight;
    public int rendSpawnWeight;
    public int dineSpawnWeight;
    public int titanSpawnWeight;
    public int marchSpawnWeight;
    public int vowSpawnWeight;

    public SyncedConfigData() { }

    public SyncedConfigData(SyncedConfig configManager)
    {
        // Settings
        powerLevel = configManager.PowerLevel;
        attackDamage = configManager.AttackDamage;
        attackSpeed = configManager.AttackSpeed;
        movementSpeed = configManager.MovementSpeed;

        // Spawn Settings
        maxSpawned = configManager.MaxSpawned;
        spawnWeightMultiplier = configManager.SpawnWeightMultiplier;
        offenseSpawnWeight = configManager.OffenseSpawnWeight;
        rendSpawnWeight = configManager.RendSpawnWeight;
        dineSpawnWeight = configManager.DineSpawnWeight;
        titanSpawnWeight = configManager.TitanSpawnWeight;
        marchSpawnWeight = configManager.MarchSpawnWeight;
        vowSpawnWeight = configManager.VowSpawnWeight;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // Settings
        serializer.SerializeValue(ref powerLevel);
        serializer.SerializeValue(ref attackDamage);
        serializer.SerializeValue(ref attackSpeed);
        serializer.SerializeValue(ref movementSpeed);

        // Spawn Settings
        serializer.SerializeValue(ref maxSpawned);
        serializer.SerializeValue(ref spawnWeightMultiplier);
        serializer.SerializeValue(ref offenseSpawnWeight);
        serializer.SerializeValue(ref rendSpawnWeight);
        serializer.SerializeValue(ref dineSpawnWeight);
        serializer.SerializeValue(ref titanSpawnWeight);
        serializer.SerializeValue(ref marchSpawnWeight);
        serializer.SerializeValue(ref vowSpawnWeight);
    }
}
