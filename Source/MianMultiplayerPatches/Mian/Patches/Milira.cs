using Milira;
using Multiplayer.API;
using Verse;

namespace MianMultiplayerPatches.Mian.Patches;

[MpPatch("ancot.milirarace")]
public class Milira
{
    public Milira(ModContentPack content)
    {
        // MP.RegisterSyncDelegateLambda(typeof(CompMilianGestateInfo), nameof(CompMilianGestateInfo.CompGetGizmosExtra), 0);
        // MP.RegisterSyncDelegateLambda(typeof(CompSwitchResonate),
        //     nameof(CompSwitchResonate.GetGizmos), 0);
    }
}