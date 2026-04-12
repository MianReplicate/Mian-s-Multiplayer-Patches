using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace MianMultiplayerPatches.Mian.Utilities;

public static class Constants
{
    // Defined when mod starts up
    public static ModContentPack ModContent;
    public static Harmony Harmony;

    public static readonly List<string> WarnOnVersion = new()
    {
    };
}