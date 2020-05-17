using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace CustomMapSizes.HarmonyPatches
{
    [HarmonyPatch(typeof(Dialog_AdvancedGameConfig), nameof(Dialog_AdvancedGameConfig.DoWindowContents))]
    static class Patch_Dialog_AdvancedGameConfig_DoWindowContents
    {
        //public static IEnumerable<CodeInstruction> MethodReplacer(this IEnumerable<CodeInstruction> instructions, MethodBase from, MethodBase to)
        //{
        //    if (from == null)
        //        throw new ArgumentException("Unexpected null argument", nameof(from));
        //    if (to == null)
        //        throw new ArgumentException("Unexpected null argument", nameof(to));

        //    foreach (var instruction in instructions)
        //    {
        //        var method = instruction.operand as MethodBase;
        //        if (method == from)
        //        {
        //            instruction.opcode = to.IsConstructor ? OpCodes.Newobj : OpCodes.Call;
        //            instruction.operand = to;
        //        }
        //        yield return instruction;
        //    }
        //}
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var from = AccessTools.Method(typeof(Listing), nameof(Listing.NewColumn));
            var to = AccessTools.Method(typeof(Patch_Dialog_AdvancedGameConfig_DoWindowContents), nameof(NewColumnPlus));

            var firstHit = false;

            foreach (var instruction in instructions)
            {

                if (!firstHit && instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method && method == from)
                {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = to;
                    firstHit = true;
                }

                yield return instruction;
            }
        }

        public static void NewColumnPlus(Listing_Standard listing)
        {
            listing.Gap(10f);
            listing.Label("CMS_Custom".Translate());
            // Marked as deprecated. But this is what the game uses.
            string customLabel = "CMS_CustomLabel".Translate(CustomMapSizesMain.mapWidth, CustomMapSizesMain.mapHeight, CustomMapSizesMain.mapWidth * CustomMapSizesMain.mapHeight);
            if (listing.RadioButton(customLabel, Find.GameInitData.mapSize == -1))
            {
                Find.GameInitData.mapSize = -1;
            }
            if (Find.GameInitData.mapSize == -1)
            {
                listing.Gap(5f);
                listing.TextFieldNumericLabeled("CMS_Width".Translate(), ref CustomMapSizesMain.mapWidth, ref CustomMapSizesMain.mapWidthBuffer);
                listing.TextFieldNumericLabeled("CMS_Height".Translate(), ref CustomMapSizesMain.mapHeight, ref CustomMapSizesMain.mapHeightBuffer);
            }
            listing.NewColumn();
        }
    }
}
