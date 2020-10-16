using Verse;

namespace CustomMapSizes
{
    public class CustomMapSizesSettings : ModSettings
    {
        public int selectedMapSize = 250;

        public int customMapSizeHeight = 250;
        public int customMapSizeWidth = 250;

        public string customMapSizeHeightBuffer = "250";
        public string customMapSizeWidthBuffer = "250";

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref selectedMapSize, $"{nameof(selectedMapSize)}");
            Scribe_Values.Look(ref customMapSizeHeight, $"{nameof(customMapSizeHeight)}");
            Scribe_Values.Look(ref customMapSizeWidth, $"{nameof(customMapSizeWidth)}");
            Scribe_Values.Look(ref customMapSizeWidthBuffer, $"{nameof(customMapSizeWidthBuffer)}");
            Scribe_Values.Look(ref customMapSizeHeightBuffer, $"{nameof(customMapSizeHeightBuffer)}");
        }
    }
}