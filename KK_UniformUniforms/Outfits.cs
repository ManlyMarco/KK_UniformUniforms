using System.Collections.Generic;

namespace KK_UniformUniforms
{
    internal static class Outfits
    {
        public enum Keys
        {
            MainTop, MainBottom, PETop, PEBottom, SwimsuitTop, SwimsuitBottom
        }

        internal static int School = 0;
        internal static int GoingHome = 1;
        internal static int PE = 2;
        internal static int Swimsuit = 3;
        internal static int Club = 4;
        internal static int Casual = 5;
        internal static int Sleep = 6;

        internal static bool SchoolFlag = true;
        internal static bool GoingHomeFlag = true;
        internal static bool PEFlag = true;
        internal static bool SwimsuitFlag = true;

        internal static int EmblemID = 0;
        internal static bool EmblemFlag = true;

        internal static Keys Current = Keys.MainTop;

        internal static List<int> GetOutfitsToChange()
        {
            // Create list
            List<int> outfitsToChange = new List<int>();

            // Add items to list based on selected values
            if (SchoolFlag) outfitsToChange.Add(School);
            if (GoingHomeFlag) outfitsToChange.Add(GoingHome);
            if (PEFlag) outfitsToChange.Add(PE);
            if (SwimsuitFlag) outfitsToChange.Add(Swimsuit);

            return outfitsToChange;
        }
    }
}
