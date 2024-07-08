namespace com.github.zehsteam.CoilHeadSettings;

internal class SpawnDataManager
{
    public static MoonSpawnDataList MoonSpawnDataList { get; private set; }

    public static void Initialize()
    {
        MoonSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.SpawnSettingsMoonList.Value);
    }

    public static SpawnData GetSpawnDataForCurrentMoon()
    {
        return MoonSpawnDataList.GetSpawnDataForCurrentMoon();
    }
}
