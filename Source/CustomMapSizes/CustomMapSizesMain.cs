using Verse;
using RimWorld;
using HarmonyLib;

namespace CustomMapSizes
{
 
    [StaticConstructorOnStartup]
    public class CustomMapSizesMain
    {
        public static int mapHeight = 250;
        public static int mapWidth = 250;

        public static string mapHeightBuffer = "250";
        public static string mapWidthBuffer = "250";

        static CustomMapSizesMain()
        {
            var harmony = new Harmony($"{nameof(CustomMapSizes)}.{nameof(CustomMapSizesMain)}");

            harmony.PatchAll();
        }
    }
}
