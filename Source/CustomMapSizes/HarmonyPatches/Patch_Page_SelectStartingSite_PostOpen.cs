using HarmonyLib;
using RimWorld;
using Verse;

namespace CustomMapSizes
{
    [HarmonyPatch(typeof(Page_SelectStartingSite), nameof(Page_SelectStartingSite.PostOpen))]
    public static class Patch_Page_SelectStartingSite_PostOpen
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