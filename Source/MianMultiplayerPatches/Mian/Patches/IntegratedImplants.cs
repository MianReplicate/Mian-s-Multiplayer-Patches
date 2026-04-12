using LTS_Implants;
using Multiplayer.API;
using Verse;

namespace MianMultiplayerPatches.Mian.Patches;

[MpPatch("lts.i")]
public class IntegratedImplants
{
    public IntegratedImplants(ModContentPack pack)
    {
        var methods =
        new[]{
            "LTS_Implants.CompDevourer_CompTick_Patch:CompTickPostfix",
            "LTS_Implants.HediffComp_ReactOnDamage_Notify_PawnPostApplyDamage_Patch_EMP:HediffComp_ReactOnDamage_Notify_PawnPostApplyDamage_Prefix"
        };
        PatchingUtilities.PatchSystemRand(methods);

        MP.RegisterSyncDelegate(typeof(LTS_FloatMenuOptionProvider_ReloadImplant), "<>c__DisplayClass6_1", "<GetOptionsFor>b__0");
        MP.RegisterSyncDelegate(typeof(LTS_FloatMenuOptionProvider_ExtractImplant), "<>c__DisplayClass9_0",
            "<GetSingleOptionFor>b__0");
    }
}