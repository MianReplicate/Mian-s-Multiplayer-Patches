using HarmonyLib;
using TemplateRW.Mian.Utilities;
using UnityEngine;
using Verse;

namespace TemplateRW.Mian;

[StaticConstructorOnStartup]
public static class Startup
{
    static Startup()
    {
        Helper.Log("A template mod to start all your moddy needs");
        
        var harmony = new Harmony("rimworld.mian.TemplateRW");
        harmony.PatchAll();
    }
}

public class TemplateSettings : ModSettings
{
    public static TemplateSettings Instance;
    public bool EnableLogging;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref EnableLogging, "EnableLogging");
        base.ExposeData();
    }
}

public class TemplateRW : Mod
{
    public TemplateRW(ModContentPack content) : base(content)
    {
        TemplateSettings.Instance = GetSettings<TemplateSettings>();
        Constants.ModContent = content;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("Template.EnableLoggingExplanation".Translate(),
            ref TemplateSettings.Instance.EnableLogging);
        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Template.ModName".Translate();
    }
}