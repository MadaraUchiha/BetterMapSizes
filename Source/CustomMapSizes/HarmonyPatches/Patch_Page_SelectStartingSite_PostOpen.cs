using HarmonyLib;
using RimWorld;
using Verse;

namespace CustomMapSizes
{
    [HarmonyPatch(typeof(Page_CreateWorldParams), nameof(Page_CreateWorldParams.PostOpen))]
    public static class Patch_Page_CreateWorldParams_PostOpen
    {
        public static void Postfix()
        {
            var mod = LoadedModManager.GetMod<CustomMapSizesMain>();
            var settings = mod.settings;

            Find.GameInitData.mapSize = settings.selectedMapSize;
            mod.CopyFromSettings(settings);
        }
    }
}