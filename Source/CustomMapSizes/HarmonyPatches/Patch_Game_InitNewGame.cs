using HarmonyLib;
using Verse;
using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using System.Reflection;

namespace CustomMapSizes.HarmonyPatches
{
    [HarmonyPatch(typeof(Game), nameof(Game.InitNewGame))]
    static class Patch_Game_InitNewGame
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var from = AccessTools.Constructor(typeof(IntVec3), new Type[] { typeof(int), typeof(int), typeof(int) });
            var to = AccessTools.Method(typeof(Patch_Game_InitNewGame), nameof(CreateMyCustomVector));

            // I could not, for the life of me, figure out how to target the correct Ldloca_S with the operand.
            // The "correct" way would be to find the one with operand == 1, but that doesn't work.
            // If you know of the way, feel free to PR/tell me about it!
            var encounteredFirstLdloca_S = false;

            var replacedInstructions = instructions.MethodReplacer(from, to);

            foreach (var instruction in replacedInstructions)
            {
                // First Ldloca_S is our vector allocation.
                // We have replaced the constructor call with a static metho, so we should get rid of this.
                // We will set it after our static call.
                if (instruction.opcode == OpCodes.Ldloca_S && !encounteredFirstLdloca_S) {
                    encounteredFirstLdloca_S = true;
                    Log.Message($"Removed the Ldloca_S! {instruction.operand}");
                    continue; 
                }
                yield return instruction;
                // If this instruction is our static call, we should set the result to the correct local variable.
                if (instruction.opcode == OpCodes.Call && (MethodInfo) instruction.operand == to)
                {
                    yield return new CodeInstruction(OpCodes.Stloc_1);
                }
            }
        }

        public static IntVec3 CreateMyCustomVector(int x, int y, int z)
        {
            if (x == -1 && z == -1)
            {
                return new IntVec3(CustomMapSizesMain.mapWidth, y, CustomMapSizesMain.mapHeight);
            }
            return new IntVec3(x, y, z);
        }
    }

}
