using Embergarden;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace MianMultiplayerPatches.Mian.Patches;

[MpPatch("breadmo.cinders")]
public class Embergarden
{
    public Embergarden(ModContentPack content)
    {
        MP.RegisterSyncMethod(typeof(CompSecondaryVerb), "SwitchVerb");
        MP.RegisterSyncMethod(typeof(Comp_TurretTransformableAbstract), nameof(Comp_TurretTransformableAbstract.TryTransform));
        MP.RegisterSyncDelegate(AccessTools.TypeByName("Embergarden.SubturretGizmo"), "<>c__DisplayClass13_0", "<GetTurretOptions>b__0");
        MP.RegisterSyncMethod(typeof(CompTransformWhenDowned), "<CompGetGizmosExtra>b__1_2");

        MP.RegisterSyncDelegate(typeof(CompUseableTargetable), "<>c__DisplayClass5_0", "<CompFloatMenuOptions>b__0");
        MP.RegisterSyncDelegate(typeof(CompUseableTargetable), "<>c__DisplayClass5_0", "<CompFloatMenuOptions>b__1");
    }
}