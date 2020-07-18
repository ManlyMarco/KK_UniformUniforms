using System.Collections.Generic;
using UnityEngine;

namespace KK_UniformUniforms
{
    internal class Colors
    {
        internal static Color Copy = new Color(1, 1, 1);

        internal static Dictionary<Outfits.Keys, Color[]> Palette = new Dictionary<Outfits.Keys, Color[]>
        {
            {
                Outfits.Keys.MainTop,
                new[]
                {
                    new Color(.9f, .9f, .9f),
                    new Color(.364f, .434f, .52f),
                    new Color(.957f, .957f, .957f),
                    new Color(.73f, .117f, .178f),
                    new Color(.364f, .809f, 1),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1),
                    new Color(.596f, .228f, .228f)
                }
            },
            {
                Outfits.Keys.MainBottom,
                new[]
                {
                    new Color(.768f, .134f, .134f),
                    new Color(.912f, .912f, .912f),
                    new Color(0, 0, 0),
                    new Color(1, 1, 1),
                    new Color(.768f, .134f, .134f),
                    new Color(.912f, .912f, .912f),
                    new Color(0, 0, 0),
                    new Color(1, 1, 1)
                }
            },
            {
                Outfits.Keys.PETop,
                new[]
                {
                    new Color(.958f, .958f, .958f),
                    new Color(.376f, .431f, .529f),
                    new Color(.494f, .621f, 1),
                    new Color(.784f, .352f, .352f),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1)
                }
            },
            {
                Outfits.Keys.PEBottom,
                new[]
                {
                    new Color(.408f, .606f, .952f),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1),
                    new Color(.408f, .606f, .952f),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1)
                }
            },
            {
                Outfits.Keys.SwimsuitTop,
                new[]
                {
                    new Color(.378f, .433f, .529f),
                    new Color(.378f, .433f, .529f),
                    new Color(.378f, .433f, .529f),
                    new Color(1, 1, 1),
                    new Color(.912f, .912f, .912f),
                    new Color(.912f, .912f, .912f),
                    new Color(1, 1, 1),
                    new Color(1, 1, 1)
                }
            },
            {
                Outfits.Keys.SwimsuitBottom,
                new[]
                {
                    new Color(.912f, .912f, .912f),
                    new Color(1, .787f, .787f),
                    new Color(1, .485f, .485f),
                    new Color(1, 1, 1),
                    new Color(.912f, .912f, .912f),
                    new Color(1, .787f, .787f),
                    new Color(1, .485f, .485f),
                    new Color(1, 1, 1)
                }
            }
        };

        internal static bool TopFlag = true;
        internal static bool BottomFlag = true;

        internal static void SetColors(ChaFileControl chaFile)
        {
            // Get list of outfits to change
            var outfitsToChange = Outfits.GetOutfitsToChange();

            // Apply colors to outfits
            foreach (var outfit in outfitsToChange)
            {
                var currParts = chaFile.coordinate[outfit].clothes.parts;

                Outfits.Keys key;
                int clothesindex;

                if (outfit == Outfits.School || outfit == Outfits.GoingHome)
                {
                    key = Outfits.Keys.MainTop;
                    clothesindex = Clothes.Top;
                }
                else if (outfit == Outfits.PE)
                {
                    key = Outfits.Keys.PETop;
                    clothesindex = Clothes.Top;
                }
                else
                {
                    key = Outfits.Keys.SwimsuitTop;
                    clothesindex = Clothes.Bra;
                }

                for (var j = 0; j < 4; j++)
                {
                    if (TopFlag)
                    {
                        currParts[clothesindex].colorInfo[j].baseColor = Palette[key][j];
                        currParts[clothesindex].colorInfo[j].patternColor = Palette[key][j + 4];
                    }

                    if (BottomFlag)
                    {
                        currParts[clothesindex + 1].colorInfo[j].baseColor = Palette[key + 1][j];
                        currParts[clothesindex + 1].colorInfo[j].patternColor = Palette[key + 1][j + 4];
                    }
                }
            }
        }
    }
}