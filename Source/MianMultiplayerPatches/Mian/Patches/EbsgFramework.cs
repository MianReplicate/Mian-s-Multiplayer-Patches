using EBSGFramework;
using Multiplayer.API;
using Verse;

namespace MianMultiplayerPatches.Mian.Patches;

[MpPatch("ebsg.framework")]
public class EbsgFramework
{
    public EbsgFramework(ModContentPack content)
    {
        MP.RegisterSyncDelegate(typeof(CompUsable_HediffModule), "<>c__DisplayClass7_1", "<CompFloatMenuOptions>b__0");
        MP.RegisterSyncDelegate(typeof(HediffComp_StageSetter), "<>c__DisplayClass7_1", "<CompGetGizmos>b__0");
        MP.RegisterSyncDelegate(typeof(CompAbilityEffect_Launch), "<>c__DisplayClass6_0", "<DropOptions>b__0");
        MP.RegisterSyncDelegate(typeof(CompAbilityEffect_Launch), "<>c__DisplayClass6_0", "<DropOptions>b__1");

        
        MP.RegisterSyncWorker<ModuleSlot>(SyncModuleSlot, typeof(ModuleSlot), shouldConstruct: true);
    }

    static void SyncModuleSlot(SyncWorker sync, ref ModuleSlot slot)
    {
        sync.Bind(ref slot.slotID);
        sync.Bind(ref slot.slotName);
        sync.Bind(ref slot.capacity);
    }
}