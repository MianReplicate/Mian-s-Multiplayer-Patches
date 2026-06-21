using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using EBSGFramework;
using HarmonyLib;
using MianMultiplayerPatches.Mian.Utilities;
using Multiplayer.API;
using Steamworks;
using Unity.Mathematics;
using UnityEngine;
using Verse;
using Constants = MianMultiplayerPatches.Mian.Utilities.Constants;

namespace MianMultiplayerPatches.Mian.Patches;

[MpPatch("ebsg.framework")]
public class EbsgFramework
{
    public EbsgFramework(ModContentPack content)
    {
        MP.RegisterSyncDelegateLocalFunc(typeof(CompUsable_HediffModule),
            nameof(CompUsable_HediffModule.CompFloatMenuOptions), "Action");
        MP.RegisterSyncDelegate(typeof(HediffComp_StageSetter), "<>c__DisplayClass7_1", "<CompGetGizmos>b__0");
        MP.RegisterSyncDelegate(typeof(CompAbilityEffect_Launch), "<>c__DisplayClass6_0", "<DropOptions>b__0");
        MP.RegisterSyncDelegate(typeof(CompAbilityEffect_Launch), "<>c__DisplayClass6_0", "<DropOptions>b__1");
        MP.RegisterSyncDelegate(typeof(Gene_Coma), "<>c__DisplayClass66_0", "<GetGizmos>b__5");
        MP.RegisterSyncDelegate(typeof(Comp_DRGConsumable), "<>c__DisplayClass4_0", "<CompFloatMenuOptions>b__0");
        MP.RegisterSyncDelegate(typeof(FloatMenuOptionProvider_ComaGene), "<>c__DisplayClass10_0", "<GetSingleOptionFor>b__0");
        MP.RegisterSyncDelegate(typeof(FloatMenuOptionProvider_ReloadableAbilities), "<>c__DisplayClass10_1", "<GetSingleOptionFor>b__0");
        
        MP.RegisterSyncDelegate(typeof(Building_SleepCasket), "<>c__DisplayClass26_0", "<GetFloatMenuOptions>b__0");
        MP.RegisterSyncDelegate(typeof(Building_SleepCasket), "<>c__DisplayClass26_0", "<GetFloatMenuOptions>b__1");
        
        MP.RegisterSyncWorker<object>(SyncModuleSlot, AccessTools.TypeByName("EBSGFramework.ModuleSlot"), shouldConstruct: true);
            
        LongEventHandler.QueueLongEvent(() =>
        {
            Constants.Harmony.Patch(AccessTools.Method(typeof(HediffComp_Modular), "RecacheGizmo"), transpiler: new HarmonyMethod(AccessTools.Method(typeof(EbsgFramework), nameof(RecacheGizmoTranspiler))));
        }, "MP.PatchingRecacheGizmo", false, null);
            
        MP.RegisterSyncMethod(AccessTools.Method(typeof(EbsgFramework), nameof(RemoveModuleWrapper)), [typeof(HediffComp_Modular), typeof(int)]);
    }
    private static void SyncModuleSlot(SyncWorker sync, ref object slot)
    {
        if (slot is ModuleSlot moduleSlot)
        {
            sync.Bind(ref moduleSlot.slotID);
            sync.Bind(ref moduleSlot.slotName);
            sync.Bind(ref moduleSlot.capacity);   
        }
    }
    
    static void RemoveModuleWrapper(HediffComp_Modular __instance, int specificModule)
    {
        __instance.RemoveModule(__instance.moduleHolder[specificModule]);
    }

    static IEnumerable<CodeInstruction> RecacheGizmoTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var matcher = new CodeMatcher(instructions);
        var context = generator.DeclareLocal(typeof(RemoveModuleContext));
        var toIncrement = generator.DeclareLocal(typeof(int));
        
        matcher.FindOpCode(OpCodes.Stloc_2);

        // Adds int i at the beginning of the loop
        matcher.Insert(
            new CodeInstruction(OpCodes.Ldc_I4_0),
            new CodeInstruction(OpCodes.Stloc, toIncrement));
        
        // Stores RemoveModuleContext and its variables
        matcher.FindOpCode(OpCodes.Br);
        matcher.Insert(
            new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(RemoveModuleContext))),
            new CodeInstruction(OpCodes.Stloc, context),
            new CodeInstruction(OpCodes.Ldloc, context),
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(RemoveModuleContext), "_instance")));
        
        // Replaces RemoveModule with my wrapper
        matcher.FindOpCode(OpCodes.Ldftn);
        matcher.Advance(-1);
        matcher.RemoveInstructions(2);
        
        matcher.Insert(
            new CodeInstruction(OpCodes.Ldloc, context),
            new CodeInstruction(OpCodes.Ldloc, toIncrement),
            new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(RemoveModuleContext), "_specificModule")),
            new CodeInstruction(OpCodes.Ldloc, context),
            new CodeInstruction(OpCodes.Ldftn,  AccessTools.Method(typeof(RemoveModuleContext), nameof(RemoveModuleContext.RemoveModule))));

        // Increments to the index we instantiated earlier
        matcher.FindOpCode(OpCodes.Ldloca_S);
        matcher.Insert(
            new CodeInstruction(OpCodes.Ldloc, toIncrement),
            new CodeInstruction(OpCodes.Ldc_I4_1),
            new CodeInstruction(OpCodes.Add),
            new CodeInstruction(OpCodes.Stloc, toIncrement)
        );
        
        return matcher.InstructionEnumeration();
    }

    public class RemoveModuleContext
    {
        private HediffComp_Modular _instance;
        private int _specificModule;
        public void RemoveModule()
        {
            RemoveModuleWrapper(_instance, _specificModule);
        }
    }
}