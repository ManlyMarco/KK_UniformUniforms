using System;
using System.Collections.Generic;
using System.Linq;
using ActionGame;
using BepInEx.Logging;
using HarmonyLib;
using Illusion.Game;
using Manager;
using UniRx;
using UnityEngine;
using Random = System.Random;

// TODO: Clean up UI class
//  - Make window size sane
//  - Further simplify

namespace KK_UniformUniforms
{
    internal class Utilities
    {
        internal static List<SaveData.CharaData> CharaList = new List<SaveData.CharaData>();

        internal static Random Rand = new Random();
        internal static PreviewClassData CurrClassData { get; set; }
        internal static ClassRoomSelectScene ClassroomScene { get; set; }

        public static Color GetSlightlyDarkerColor(Color incolor)
        {
            Color.RGBToHSV(incolor, out var h, out var s, out var v);
            v = v > 0.08f ? v - 0.08f : 0;
            return Color.HSVToRGB(h, s, v);
        }

        internal static void SetDarkerPatternColors()
        {
            foreach (var key in (Outfits.Keys[])Enum.GetValues(typeof(Outfits.Keys)))
                for (var j = 0; j < 4; j++)
                    Colors.Palette[key][j + 4] = GetSlightlyDarkerColor(Colors.Palette[key][j]);
        }

        private static void ClearCharList()
        {
            while (CharaList.Count > 0) CharaList.RemoveAt(CharaList.Count - 1);
        }

        internal static void LoadCharList()
        {
            // Clear list
            ClearCharList();

            // Load current data from heroineList and new characters from charaList
            var heroineList = Singleton<Game>.Instance.HeroineList;
            var classList = Traverse.Create(ClassroomScene.classRoomList).Field("charaList")
                .GetValue<List<SaveData.CharaData>>();
            foreach (var heroine in heroineList) CharaList.Add(heroine);
            for (var i = heroineList.Count; i < classList.Count; i++)
                if (classList[i].GetType() != typeof(SaveData.Player))
                    CharaList.Add(classList[i]);
        }

        internal static void LoadColorsFromCharacter(bool showMessages = true)
        {
            if (CurrClassData.data == null) return;
            var chaFile = CurrClassData.data.charFile;

            // Load strict uniforms if character is wearing uniform
            if (chaFile.coordinate[0].clothes.parts[0].id == 1 || chaFile.coordinate[0].clothes.parts[0].id == 2)
            {
                Clothes.StrictUniform = chaFile.coordinate[0].clothes.parts[0].id;
                if (showMessages)
                    KK_UniformUniforms.Logger.Log(LogLevel.Message,
                        $"Setting uniform to apply to {(Clothes.StrictUniform == 2 ? "Blazer" : "Sailor")}.");
            }
            else
            {
                Clothes.StrictUniform = 0;
                if (showMessages)
                    KK_UniformUniforms.Logger.Log(LogLevel.Message,
                        "Current card is not wearing regulation uniform, setting uniform to Sailor/Blazer.");
            }

            // Get list of outfits to change
            var outfitsToChange = Outfits.GetOutfitsToChange();

            // Load colors from outfits
            foreach (var outfit in outfitsToChange)
            {
                var currParts = chaFile.coordinate[outfit].clothes.parts;
                for (var i = 0; i < 4; i++)
                {
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

                    if (Colors.TopFlag)
                    {
                        Colors.Palette[key][i] = currParts[clothesindex].colorInfo[i].baseColor;
                        Colors.Palette[key][i + 4] = currParts[clothesindex].colorInfo[i].patternColor;
                    }

                    if (Colors.BottomFlag)
                    {
                        Colors.Palette[key + 1][i] = currParts[clothesindex + 1].colorInfo[i].baseColor;
                        Colors.Palette[key + 1][i + 4] = currParts[clothesindex + 1].colorInfo[i].baseColor;
                    }
                }
            }

            if (showMessages)
                Utils.Sound.Play(SystemSE.sel);
        }

        private static void SetRandomClothes(ChaFileControl chaFile)
        {
            // Get list of outfits to change
            var outfitsToChange = Outfits.GetOutfitsToChange();

            // Apply clothes to outfits
            foreach (var outfit in outfitsToChange)
            {
                var currParts = chaFile.coordinate[outfit].clothes.parts;
                var currSubParts = chaFile.coordinate[outfit].clothes.subPartsId;
                var topChanged = false;

                currParts[Clothes.Top].emblemeId = Outfits.EmblemFlag ? Outfits.EmblemID : currParts[Clothes.Top].emblemeId;

                List<int> tops;
                List<int> bottoms;

                if (outfit == Outfits.School || outfit == Outfits.GoingHome)
                {
                    tops = Default.Top;
                    bottoms = Default.Bottom;
                }
                else if (outfit == Outfits.PE)
                {
                    tops = Default.PETop;
                    bottoms = Default.PEBottom;
                }
                else
                {
                    tops = Default.SwimsuitTop;
                    bottoms = Default.SwimsuitBottom;
                }

                // Change Tops
                if (Clothes.StrictUniform == 0)
                {
                    if (Clothes.TopFlag && !tops.Contains(currParts[Clothes.Top].id))
                    {
                        currParts[Clothes.Top].id = tops[Rand.Next(tops.Count)];
                        topChanged = true;
                    }
                }
                else
                {
                    currParts[Clothes.Top].id = Clothes.StrictUniform;
                    topChanged = true;
                }

                // Change Bottoms
                if (Clothes.BottomFlag && !bottoms.Contains(currParts[Clothes.Bottom].id))
                    currParts[Clothes.Bottom].id = bottoms[Rand.Next(bottoms.Count)];

                // Change Swimsuit
                if (outfit == Outfits.Swimsuit)
                    currParts[Clothes.Bra].id = Default.SwimsuitBras[Rand.Next(Default.SwimsuitBras.Count)];

                List<int> subunder;
                List<int> subover;
                List<int> subdeco;

                // Set random subparts
                if (outfit == Outfits.School || outfit == Outfits.GoingHome)
                {
                    if (currParts[Clothes.Top].id == 2)
                    {
                        subunder = Default.SubBlazerUnder;
                        subover = Default.SubBlazerOver;
                        subdeco = Default.SubBlazerdeco;
                    }
                    else
                    {
                        subunder = Default.SubSailorUnder;
                        subover = Default.SubSailorOver;
                        subdeco = Default.SubSailorDeco;
                    }
                }
                else if (outfit == Outfits.PE)
                {
                    subunder = Default.SubPEUnder;
                    subover = Default.SubPEOver;
                    subdeco = Default.SubPEDeco;
                }
                else
                {
                    subunder = Default.SubSwimsuitUnder;
                    subover = Default.SubSwimsuitOver;
                    subdeco = Default.SubSwimsuitDeco;
                }


                if (topChanged)
                {
                    currSubParts[Clothes.Subshirt] = subunder[Rand.Next(subunder.Count)];
                    currSubParts[Clothes.Subjacketcollar] = subover[Rand.Next(subover.Count)];
                    currSubParts[Clothes.Subdecoration] = subdeco[Rand.Next(subdeco.Count)];
                }
            }

            // Set Colors
            Colors.SetColors(chaFile);
        }


        internal static void ApplySettings(Apply apply)
        {
            // Refresh character list
            LoadCharList();

            // Set counter for number of cards applied to
            var appliedNo = 0;

            // Get currently viewed ClassList
            var currSchoolClass = Traverse.Create(ClassroomScene.classRoomList).Field("_page").GetValue<IntReactiveProperty>().Value;

            // If character is Minase Ai, add her to players class
            CharaList.Find(x => ((SaveData.Heroine)x).fixCharaID == -10).schoolClass = 1;

            foreach (var chaData in apply == Apply.Class
                ? CharaList.Where(c => c.schoolClass == currSchoolClass)
                : CharaList)
            {
                if (apply == Apply.Card && CurrClassData != null &&
                    chaData.schoolClassIndex != CurrClassData.data.schoolClassIndex) continue;
                if (!((SaveData.Heroine)chaData).isTeacher)
                {
                    if (currSchoolClass == 4)
                    {
                        // Set cards for Fixed Characters
                        System.Diagnostics.Debug.Assert(CurrClassData != null, nameof(CurrClassData) + " != null");
                        SetRandomClothes(CurrClassData.data.charFile);
                        appliedNo++;
                        if (apply == Apply.Card) break;
                    }
                    else
                    {
                        SetRandomClothes(chaData.charFile);
                        appliedNo++;
                    }
                }

                if (chaData.chaCtrl != null) chaData.chaCtrl.chaFile.coordinate = chaData.charFile.coordinate;
            }

            // Reset Minase Ai to fixed character list
            CharaList.Find(x => ((SaveData.Heroine)x).fixCharaID == -10).schoolClass = -1;

            // Apply colors to player
            var player = Singleton<Game>.Instance.Player;
            if (apply != Apply.Card)
                if (apply == Apply.All || currSchoolClass == player.schoolClass)
                {
                    Colors.SetColors(player.charFile);
                    appliedNo++;
                    if (player.chaCtrl != null) player.chaCtrl.chaFile.coordinate = player.charFile.coordinate;
                }

            // Play sound and write message
            Utils.Sound.Play(SystemSE.ok_l);
            KK_UniformUniforms.Logger.Log(LogLevel.Message,
                $"Successfully changed clothes for {appliedNo} characters!");
        }

        internal enum Apply
        {
            All,
            Class,
            Card
        }
    }
}