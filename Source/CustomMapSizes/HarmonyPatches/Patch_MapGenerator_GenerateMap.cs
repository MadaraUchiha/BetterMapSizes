using HarmonyLib;
using Verse;

namespace CustomMapSizes.HarmonyPatches
{
    /// <summary>
    /// Gives the starting map its exact custom width x height. Vanilla only builds square
    /// maps from GameInitData.mapSize, so Patch_Game_InitNewGame passes a square placeholder
    /// and this swaps in the real dimensions, which also allows non-square maps.
    /// </summary>
    [HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.GenerateMap))]
    static class Patch_MapGenerator_GenerateMap
    {
        public static void Prefix(ref IntVec3 mapSize)
        {
            if (!Patch_Game_InitNewGame.startingMapPending || !Patch_Game_InitNewGame.customMapSize.HasValue)
            {
                return;
            }

            var placeholder = Patch_Game_InitNewGame.placeholderSize;
            if (mapSize.x != placeholder || mapSize.z != placeholder)
            {
                return;
            }

            mapSize = Patch_Game_InitNewGame.customMapSize.Value;
            Patch_Game_InitNewGame.startingMapPending = false;
        }
    }
}
