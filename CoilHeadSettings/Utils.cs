namespace com.github.zehsteam.CoilHeadSettings;

public class Utils
{
    public static bool TryGetEnemyType(string enemyName, out EnemyType enemyType)
    {
        enemyType = GetEnemyType(enemyName);
        return enemyType != null;
    }

    public static EnemyType GetEnemyType(string enemyName)
    {
        foreach (var level in StartOfRound.Instance.levels)
        {
            if (TryGetEnemyTypeInLevel(level, enemyName, out EnemyType enemyType))
            {
                return enemyType;
            }
        }

        return null;
    }

    public static bool TryGetEnemyTypeInLevel(SelectableLevel level, string enemyName, out EnemyType enemyType)
    {
        enemyType = GetEnemyTypeInLevel(level, enemyName);
        return enemyType != null;
    }

    public static EnemyType GetEnemyTypeInLevel(SelectableLevel level, string enemyName)
    {
        SpawnableEnemyWithRarity spawnableEnemyWithRarity = level.Enemies.Find(_ => _.enemyType.enemyName == enemyName);
        if (spawnableEnemyWithRarity == null) return null;

        return spawnableEnemyWithRarity.enemyType;
    }

    public static bool TryGetLevelByPlanetName(string planetName, out SelectableLevel level)
    {
        level = GetLevelByPlanetName(planetName);
        return level != null;
    }

    public static SelectableLevel GetLevelByPlanetName(string planetName)
    {
        foreach (var level in StartOfRound.Instance.levels)
        {
            if (level.PlanetName == planetName)
            {
                return level;
            }
        }

        return null;
    }
}
