namespace MianMultiplayerPatches.Mian.Utilities;

public static class Helper
{
    private static string AppendModID(string text)
    {
        return $"[{GetModName()}] " + text;
    }
    public static void Log(string text)
    {
        Verse.Log.Message(AppendModID(text));
    }

    public static void Debug(string text)
    {
        if (ModSettings.Instance.EnableLogging)
            Verse.Log.Message(AppendModID(text));
    }

    public static void Error(string text)
    {
        Verse.Log.Error(AppendModID(text));
    }

    public static string GetModVersion()
    {
        return Constants.ModContent.ModMetaData.ModVersion;
    }
    
    public static string GetModName()
    {
        return Constants.ModContent.ModMetaData.Name;
    }
}