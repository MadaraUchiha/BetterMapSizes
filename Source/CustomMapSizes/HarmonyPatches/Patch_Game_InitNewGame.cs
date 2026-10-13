using HarmonyLib;
using System;
using Verse;

namespace CustomMapSizes.HarmonyPatches
{
    /// <summary>
    /// The "Custom" option is stored as GameInitData.mapSize == -1. Vanilla builds the
    /// starting map from new IntVec3(mapSize, 1, mapSize), so the -1 has to be replaced
    /// before InitNewGame runs.
    ///
    /// This used to be a Transpiler that rewrote the IntVec3 constructor call inside
    /// InitNewGame. That broke whenever another mod also transpiled InitNewGame (for example
    /// Vanilla Expanded Framework): the -1 reached vanilla and map allocation threw
    /// System.OverflowException. Prefix/Postfix patches don't depend on the method's IL,
    /// so they keep working regardless of what other mods do to it.
    /// </summary>
    [HarmonyPatch(typeof(Game), nameof(Game.InitNewGame))]
    static class Patch_Game_InitNewGame
    {
        // The exact custom size for this new game, or null when a preset size is in use.
        internal static IntVec3? customMapSize;

        // Square size handed to vanilla in place of the -1. The starting map is the
        // GenerateMap call that receives exactly this size.
        internal static int placeholderSize;

        // True until the starting map has been given its exact custom size.
        internal static bool startingMapPending;

        public static void Prefix()
        {
            customMapSize = null;
            startingMapPending = false;

            var initData = Find.GameInitData;
            if (initData == null || initData.mapSize != -1)
            {
                return;
            }

            var width = CustomMapSizesMain.mapWidth;
            var height = CustomMapSizesMain.mapHeight;
            if (width <= 0 || height <= 0)
            {
                Log.Warning($"[Better Map Sizes] Invalid custom map size {width}x{height}, using 250x250 instead.");
                width = 250;
                height = 250;
            }

            placeholderSize = Math.Max(width, height);
            initData.mapSize = placeholderSize;
            customMapSize = new IntVec3(width, 1, height);
            startingMapPending = true;
        }

        public static void Postfix()
        {
            // New settlements founded later in the game use the world's initial map size.
            // Vanilla set it from the square placeholder, so store the real width x height.
            if (customMapSize.HasValue && Find.World != null)
            {
                Find.World.info.initialMapSize = customMapSize.Value;
            }
        }

        public static void Finalizer()
        {
            // Runs even if InitNewGame throws, so the custom size can't leak into a later game.
            customMapSize = null;
            startingMapPending = false;
        }
    }
}
