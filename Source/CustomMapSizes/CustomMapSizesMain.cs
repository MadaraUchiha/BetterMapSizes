using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace CustomMapSizes
{

    public class CustomMapSizesMain : Mod
    {
        public CustomMapSizesSettings settings;

        public static int mapHeight = 250;
        public static int mapWidth = 250;

        public static string mapHeightBuffer = "250";
        public static string mapWidthBuffer = "250";

        public override string SettingsCategory() => "Custom Map Sizes";
        public CustomMapSizesMain(ModContentPack content) : base(content)
        {
            settings = GetSettings<CustomMapSizesSettings>();
            var harmony = new Harmony($"{nameof(CustomMapSizes)}.{nameof(CustomMapSizesMain)}");

            harmony.PatchAll();
        }

        public void CopyFromSettings(CustomMapSizesSettings settings) {
            mapHeight = settings.customMapSizeHeight;
            mapWidth = settings.customMapSizeWidth;
            mapHeightBuffer = settings.customMapSizeHeightBuffer;
            mapWidthBuffer = settings.customMapSizeWidthBuffer;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var labelRect = inRect;
            labelRect.yMin += 30f;
            Widgets.Label(labelRect, "CMS_Settings_Description".Translate());

            var listingRect = labelRect;
            listingRect.yMin += 30f;

            var listing = new Listing_Standard();
            listing.ColumnWidth = 250f;
            listing.Begin(listingRect);

            var supportedSizes = new List<int>() { 200, 225, 250, 275, 300, 325 };
            foreach (var size in supportedSizes)
            {
                var label = "MapSizeDesc".Translate(size, size * size);
#pragma warning disable 612, 618
                if (listing.RadioButton(label, settings.selectedMapSize == size))
                {
#pragma warning restore 612, 618
                    settings.selectedMapSize = size;
                }
            }
            listing.Gap(10f);
            listing.Label("CMS_Custom".Translate());
            // Marked as deprecated. But this is what the game uses.
            string customLabel = "CMS_CustomLabel".Translate(settings.customMapSizeWidth, settings.customMapSizeHeight, settings.customMapSizeWidth * settings.customMapSizeHeight);
#pragma warning disable 612, 618
            if (listing.RadioButton(customLabel, settings.selectedMapSize == -1))
#pragma warning restore 612, 618

            {
                settings.selectedMapSize = -1;
            }
            if (settings.selectedMapSize == -1)
            {
                listing.Gap(5f);
                listing.TextFieldNumericLabeled("CMS_Width".Translate(), ref settings.customMapSizeWidth, ref settings.customMapSizeWidthBuffer);
                listing.TextFieldNumericLabeled("CMS_Height".Translate(), ref settings.customMapSizeHeight, ref settings.customMapSizeHeightBuffer);
            }

            listing.End();

            settings.Write();
        }

    }
}
