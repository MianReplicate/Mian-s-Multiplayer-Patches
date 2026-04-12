using MianMultiplayerPatches.Mian.Utilities;
using RimWorld;
using Verse;

namespace MianMultiplayerPatches.Mian.GameComps;

public class VersionTracker : GameComponent
{
    public string lastLaunchedVersion = "0.0";
    
    public static VersionTracker Instance => Current.Game.GetComponent<VersionTracker>();

    public VersionTracker(Game game)
    {
        
    }

    public override void StartedNewGame() => lastLaunchedVersion = Helper.GetModVersion();

    public override void LoadedGame()
    {
        if (!lastLaunchedVersion.Equals(Helper.GetModVersion()))
        {
            lastLaunchedVersion = Helper.GetModVersion();
            
            if(Constants.WarnOnVersion.Contains(lastLaunchedVersion))
                Find.LetterStack.ReceiveLetter("Template.VersionUpdate".Translate(), "Template.VersionUpdateMain".Translate(), LetterDefOf.NegativeEvent);
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref lastLaunchedVersion, "lastLaunchedVersion", "0.0");
        if (Scribe.mode == LoadSaveMode.LoadingVars)
            Helper.Log("Last loaded " + Helper.GetModVersion()+ " version: " + lastLaunchedVersion);
    }
}