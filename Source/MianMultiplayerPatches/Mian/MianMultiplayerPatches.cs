using System;
using System.Linq;
using HarmonyLib;
using MianMultiplayerPatches.Mian.Utilities;
using Multiplayer.API;
using UnityEngine;
using Verse;

namespace MianMultiplayerPatches.Mian;

public class ModSettings : Verse.ModSettings
{
    public static ModSettings Instance;
    public bool EnableLogging;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref EnableLogging, "EnableLogging");
        base.ExposeData();
    }
}

public class MianMultiplayerPatches : Mod
{
    public MianMultiplayerPatches(ModContentPack content) : base(content)
    {
        ModSettings.Instance = GetSettings<ModSettings>();
        Constants.ModContent = content;
        Constants.Harmony = new Harmony("rimworld.mian.MianMultiplayer");

        if (!MP.enabled)
        {
            Helper.Log("Multiplayer mod not detected! Did you put this mod after it in the mod list?");
            return;
        }
        
        Helper.Log("Patching up dem mods for all your multiplayer needs :D");

        var patches = content.assemblies.loadedAssemblies.SelectMany(a => a.GetTypes()).Where(t => t.HasAttribute<MpPatch>())
            .SelectMany(t => (MpPatch[])t.GetCustomAttributes(typeof(MpPatch), false), resultSelector: (type, patch) =>
                new { type, patch })
            .Join(LoadedModManager.RunningMods, 
                assembly => assembly.patch.PackageId.ToLower(),
                mod => mod.PackageId.Replace("_steam", "").Replace("_copy", ""),
                (assembly, mod) => new {assembly.type, mod});
        
        foreach (var assembly in patches)
        {
            try
            {
                Activator.CreateInstance(assembly.type, assembly.mod);
                
                Helper.Log($"Initialized compatibility for {assembly.mod.ModMetaData.Name}");
            }
            catch (Exception e)
            {
                Helper.Error($"Exception occurred while loading {assembly.mod.ModMetaData.Name}: {e.InnerException}");
            }
        }
        
        Constants.Harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("MianMultiplayer.EnableLoggingExplanation".Translate(),
            ref ModSettings.Instance.EnableLogging);
        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "MianMultiplayer.ModName".Translate();
    }
}

public class MpPatch : Attribute
{
    public string PackageId { get; }

    public MpPatch(string packageId)
    {
        this.PackageId = packageId;
    }
    
    public override object TypeId
    {
        get { return this; }
    }
}