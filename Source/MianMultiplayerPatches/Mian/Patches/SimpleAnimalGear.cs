using HarmonyLib;
using Multiplayer.API;
using SimpleAnimalGear;
using Verse;

namespace MianMultiplayerPatches.Mian.Patches;

[MpPatch("aelanna.simpleanimalgear")]
public class SimpleAnimalGear
{
    public SimpleAnimalGear(ModContentPack pack)
    {
        MP.RegisterSyncMethod(AccessTools.Method(typeof(CompAnimalGear), "StartJob"), [typeof(Pawn), typeof(Pawn)]);
        MP.RegisterSyncMethod(AccessTools.Method(typeof(HediffComp_AnimalGear), "StartJob"), [typeof(Pawn)]);
    }
}