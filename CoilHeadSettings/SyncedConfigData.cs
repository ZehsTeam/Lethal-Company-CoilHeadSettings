using System;
using Unity.Netcode;

namespace com.github.zehsteam.CoilHeadSettings;

[Serializable]
internal class SyncedConfigData : INetworkSerializable
{
    // Settings
    public float powerLevel;
    public int attackDamage;
    public float attackSpeed;
    public float movementSpeed;

    // Spawn Settings
    public int maxSpawnCount;
    public float spawnWeightMultiplier;
    public int liquidationSpawnWeight;
    public int embrionSpawnWeight;
    public int artificeSpawnWeight;
    public int titanSpawnWeight;
    public int dineSpawnWeight;
    public int rendSpawnWeight;
    public int adamanceSpawnWeight;
    public int offenseSpawnWeight;
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
        maxSpawnCount = configManager.MaxSpawnCount;
        spawnWeightMultiplier = configManager.SpawnWeightMultiplier;
        liquidationSpawnWeight = configManager.LiquidationSpawnWeight;
        embrionSpawnWeight = configManager.EmbrionSpawnWeight;
        artificeSpawnWeight = configManager.ArtificeSpawnWeight;
        titanSpawnWeight = configManager.TitanSpawnWeight;
        dineSpawnWeight = configManager.DineSpawnWeight;
        rendSpawnWeight = configManager.RendSpawnWeight;
        adamanceSpawnWeight = configManager.AdamanceSpawnWeight;
        offenseSpawnWeight = configManager.OffenseSpawnWeight;
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
        serializer.SerializeValue(ref maxSpawnCount);
        serializer.SerializeValue(ref spawnWeightMultiplier);
        serializer.SerializeValue(ref liquidationSpawnWeight);
        serializer.SerializeValue(ref embrionSpawnWeight);
        serializer.SerializeValue(ref artificeSpawnWeight);
        serializer.SerializeValue(ref titanSpawnWeight);
        serializer.SerializeValue(ref dineSpawnWeight);
        serializer.SerializeValue(ref rendSpawnWeight);
        serializer.SerializeValue(ref adamanceSpawnWeight);
        serializer.SerializeValue(ref offenseSpawnWeight);
        serializer.SerializeValue(ref marchSpawnWeight);
        serializer.SerializeValue(ref vowSpawnWeight);
    }
}
