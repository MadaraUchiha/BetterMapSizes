using Verse;
using RimWorld;
using HarmonyLib;

namespace CustomMapSizes
{
 
    [StaticConstructorOnStartup]
    public class Main
    {
        public static int mapHeight = 250;
        public static int mapWidth = 250;

        public static string mapHeightTooltip = "250";
        public static string mapWidthTooltip = "250";

        static Main()
        {
            var harmony = new Harmony($"{nameof(CustomMapSizes)}.{nameof(Main)}");

            harmony.PatchAll();
        }
    }
}
