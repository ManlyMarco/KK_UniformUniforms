using UnityEngine;

namespace KK_UniformUniforms
{
    internal class UI
    {
        internal static bool Visible = false;

        private static bool HSV;
        private static bool GUIChanged;
        private static bool Advanced;

        private static readonly int ToggleWidth = 57;
        private static readonly int Offset = 165;
        private static readonly int AdvancedMenuHeight = 437;
        private static readonly int Width = 170;
        private static readonly int WidthLim = 160;

        private static readonly int MinHeight = 1080 - Offset;
        private static readonly int MaxHeight = (int)(Screen.height * .92f);
        private static int HeightOffset;
        private static int HeightOffsetAdv;

        internal static Vector2 ScrollPos { get; set; }
        internal static Texture2D PreviewTexture { get; set; }

        internal static Rect GetWindowRect()
        {
            // Calculate advanced window height
            var advancedHeight = (MinHeight >= MaxHeight ? MaxHeight : MinHeight) - HeightOffset;

            return new Rect(
                5, // Window x
                5, // Window y
                Advanced && advancedHeight != MinHeight ? // If advanced color controls and not low res...
                    Width : // ...set standard width
                    Width - 24, // ...adjust width for scrollbar
                Advanced ? // If advanced color controls...
                    advancedHeight - HeightOffsetAdv : // ...set height to advanced - offsets from selections
                    MinHeight - AdvancedMenuHeight // ...set height to minimum - difference in height to advanced menu
            );
        }

        private static void DrawOutfitsToChange()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Outfits to change");
                Outfits.SchoolFlag = GUILayout.Toggle(Outfits.SchoolFlag, "School");
                Outfits.GoingHomeFlag = GUILayout.Toggle(Outfits.GoingHomeFlag, "Going Home");
                Outfits.PEFlag = GUILayout.Toggle(Outfits.PEFlag, "PE");
                Outfits.SwimsuitFlag = GUILayout.Toggle(Outfits.SwimsuitFlag, "Swimsuit");
            }
            GUILayout.EndVertical();
        }

        private static void DrawPartsToChange()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Parts to change");
                GUILayout.BeginHorizontal();
                Clothes.TopFlag = GUILayout.Toggle(Clothes.TopFlag, "Top", GUILayout.Width(ToggleWidth));
                Clothes.BottomFlag = GUILayout.Toggle(Clothes.BottomFlag, "Bottom", GUILayout.Width(ToggleWidth));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private static void DrawColorsToChange()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Colors to change");
                GUILayout.BeginHorizontal();
                Colors.TopFlag = GUILayout.Toggle(Colors.TopFlag, "Top", GUILayout.Width(ToggleWidth));
                Colors.BottomFlag = GUILayout.Toggle(Colors.BottomFlag, "Bottom", GUILayout.Width(ToggleWidth));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private static void DrawUniformToApply()
        {
            if (Clothes.TopFlag)
            {
                HeightOffset = 0;
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    GUILayout.Label("Uniform to apply");
                    GUILayout.BeginHorizontal();
                    {
                        Clothes.StrictUniform = GUILayout.Toggle(Clothes.StrictUniform == 1, "Sailor", GUILayout.Width(ToggleWidth))
                                ? 1
                                : Clothes.StrictUniform;
                        Clothes.StrictUniform = GUILayout.Toggle(Clothes.StrictUniform == 2, "Blazer", GUILayout.Width(ToggleWidth))
                                ? 2
                                : Clothes.StrictUniform;
                    }
                    GUILayout.EndHorizontal();
                    Clothes.StrictUniform = GUILayout.Toggle(Clothes.StrictUniform == 0, "Sailor or Blazer")
                        ? 0
                        : Clothes.StrictUniform;
                }
                GUILayout.EndVertical();
            }
            else
            {
                HeightOffset = 24 * 3 + 5;
            }
        }

        private static void DrawAdvancedSelectOutfit()
        {
            GUILayout.Label("Select Outfit");
            GUILayout.BeginHorizontal();
            {
                Outfits.Current =
                    GUILayout.Toggle(
                        Outfits.Current == Outfits.Keys.MainTop || Outfits.Current == Outfits.Keys.MainBottom, "School",
                        GUILayout.Width(ToggleWidth))
                        ? Outfits.Keys.MainTop
                        : Outfits.Current;
                Outfits.Current =
                    GUILayout.Toggle(Outfits.Current == Outfits.Keys.PETop || Outfits.Current == Outfits.Keys.PEBottom,
                        "PE", GUILayout.Width(ToggleWidth))
                        ? Outfits.Keys.PETop
                        : Outfits.Current;
            }
            GUILayout.EndHorizontal();
            Outfits.Current =
                GUILayout.Toggle(
                    Outfits.Current == Outfits.Keys.SwimsuitTop || Outfits.Current == Outfits.Keys.SwimsuitBottom,
                    "Swimsuit")
                    ? Outfits.Keys.SwimsuitTop
                    : Outfits.Current;
        }

        private static void DrawAdvancedSelectColorIndex()
        {
            GUILayout.Label("Select Part Color");
            GUILayout.BeginHorizontal();
            {
                Clothes.Current = GUILayout.Toggle(Clothes.Current == 0 || Clothes.Current == 4, "1")
                    ? 0
                    : Clothes.Current;
                Clothes.Current = GUILayout.Toggle(Clothes.Current == 1 || Clothes.Current == 5, "2")
                    ? 1
                    : Clothes.Current;
                Clothes.Current = GUILayout.Toggle(Clothes.Current == 2 || Clothes.Current == 6, "3")
                    ? 2
                    : Clothes.Current;
                Clothes.Current = GUILayout.Toggle(Clothes.Current == 3 || Clothes.Current == 7, "4")
                    ? 3
                    : Clothes.Current;
            }
            GUILayout.EndHorizontal();
        }

        private static void DrawAdvancedSelectPart()
        {
            if (Outfits.Current != Outfits.Keys.SwimsuitTop)
            {
                HeightOffsetAdv = 0;
                GUILayout.BeginHorizontal();
                Clothes.BottomFlag = !GUILayout.Toggle(!Clothes.BottomFlag, "Top", GUILayout.Width(ToggleWidth));
                Clothes.BottomFlag = GUILayout.Toggle(Clothes.BottomFlag, "Bottom", GUILayout.Width(ToggleWidth));
                GUILayout.EndHorizontal();
                if (Clothes.BottomFlag) Outfits.Current += 1;
            }
            else
            {
                HeightOffsetAdv = 22;
            }
        }

        private static void DrawAdvancedSelectPattern()
        {
            GUILayout.BeginHorizontal();
            {
                Clothes.PatternFlag = !GUILayout.Toggle(!Clothes.PatternFlag, "Main");
                Clothes.PatternFlag = GUILayout.Toggle(Clothes.PatternFlag, "Pattern");
            }
            GUILayout.EndHorizontal();
            if (Clothes.PatternFlag) Clothes.Current += 4;
        }

        private static void DrawAdvancedColorPicker()
        {
            // Check if sliders are HSV or RGB and draw sliders
            if (HSV)
            {
                Color.RGBToHSV(Colors.Palette[Outfits.Current][Clothes.Current], out var h, out var s, out var v);
                var hsv = SetGUISliderTextures(h, s, v, true);
                Colors.Palette[Outfits.Current][Clothes.Current] = Color.HSVToRGB(hsv[0], hsv[1], hsv[2]);
            }
            else
            {
                var rgb = SetGUISliderTextures(
                    Colors.Palette[Outfits.Current][Clothes.Current].r,
                    Colors.Palette[Outfits.Current][Clothes.Current].g,
                    Colors.Palette[Outfits.Current][Clothes.Current].b
                );
                Colors.Palette[Outfits.Current][Clothes.Current].r = rgb[0];
                Colors.Palette[Outfits.Current][Clothes.Current].g = rgb[1];
                Colors.Palette[Outfits.Current][Clothes.Current].b = rgb[2];
            }

            Colors.Palette[Outfits.Current][Clothes.Current].a = 1;

            // HSV / RGB Selector
            if (GUILayout.Button(HSV ? "To RGB" : "To HSV")) HSV = !HSV;

            // Refresh Color Preview Texture
            for (var x = 1; x <= 65; x++)
                for (var y = 1; y <= 65; y++)
                    PreviewTexture.SetPixel(x, y, Colors.Palette[Outfits.Current][Clothes.Current]);
            PreviewTexture.Apply();

            // Draw Color preview
            GUILayout.Box(PreviewTexture);
        }

        private static void AdvancedColor()
        {
            DrawAdvancedSelectOutfit();

            DrawAdvancedSelectColorIndex();

            DrawAdvancedSelectPart();

            DrawAdvancedSelectPattern();

            DrawAdvancedColorPicker();

            // Draw Buttons
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy")) Colors.Copy = Colors.Palette[Outfits.Current][Clothes.Current];

            if (GUILayout.Button("Paste")) Colors.Palette[Outfits.Current][Clothes.Current] = Colors.Copy;

            GUILayout.EndHorizontal();
            if (GUILayout.Button("Auto Pattern Colors")) Utilities.SetDarkerPatternColors();
        }

        private static float[] SetGUISliderTextures(float x, float y, float z, bool hsv = false)
        {
            // Save standard textures for restoring later
            var normal = GUI.skin.horizontalSlider.normal.background;
            var active = GUI.skin.horizontalSlider.active.background;
            var hover = GUI.skin.horizontalSlider.hover.background;
            var focused = GUI.skin.horizontalSlider.focused.background;

            // Make slider textures for each color dimension
            var xTex = new Texture2D(1, 1);
            var yTex = new Texture2D(1, 1);
            var zTex = new Texture2D(1, 1);

            // Set colors for each slider
            if (hsv)
            {
                xTex.SetPixel(1, 1, Color.HSVToRGB(x, 1, 1));
                yTex.SetPixel(1, 1, Color.HSVToRGB(x, y, 1));
                zTex.SetPixel(1, 1, Color.HSVToRGB(x, y, z));
            }
            else
            {
                xTex.SetPixel(1, 1, new Color(x, 0, 0));
                yTex.SetPixel(1, 1, new Color(0, y, 0));
                zTex.SetPixel(1, 1, new Color(0, 0, z));
            }

            xTex.Apply();
            yTex.Apply();
            zTex.Apply();

            // Draw slider colors
            GUILayout.Label(hsv ? "Hue" : "Red");
            GUI.skin.horizontalSlider.normal.background = xTex;
            GUI.skin.horizontalSlider.active.background = xTex;
            GUI.skin.horizontalSlider.hover.background = xTex;
            GUI.skin.horizontalSlider.focused.background = xTex;
            x = GUILayout.HorizontalSlider(x, 0, 1);

            GUILayout.Label(hsv ? "Saturation" : "Green");
            GUI.skin.horizontalSlider.normal.background = yTex;
            GUI.skin.horizontalSlider.active.background = yTex;
            GUI.skin.horizontalSlider.hover.background = yTex;
            GUI.skin.horizontalSlider.focused.background = yTex;
            y = GUILayout.HorizontalSlider(y, 0, 1);

            GUILayout.Label(hsv ? "Value" : "Blue");
            GUI.skin.horizontalSlider.normal.background = zTex;
            GUI.skin.horizontalSlider.active.background = zTex;
            GUI.skin.horizontalSlider.hover.background = zTex;
            GUI.skin.horizontalSlider.focused.background = zTex;
            z = GUILayout.HorizontalSlider(z, 0, 1);

            // Reset original textures to GUI standards
            GUI.skin.horizontalSlider.normal.background = normal;
            GUI.skin.horizontalSlider.active.background = active;
            GUI.skin.horizontalSlider.hover.background = hover;
            GUI.skin.horizontalSlider.focused.background = focused;
            return new[] { x, y, z };
        }

        internal static void DrawMainGUI(int id)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(20);
                ScrollPos = GUILayout.BeginScrollView(ScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar,
                    GUILayout.ExpandHeight(true), GUILayout.MaxHeight(MaxHeight - 22));
                {
                    GUILayout.BeginHorizontal(GUILayout.MaxWidth(WidthLim));
                    {
                        GUILayout.Space(5);
                        GUILayout.BeginVertical();
                        {
                            if (GUILayout.Button("Load From Selected")) Utilities.LoadColorsFromCharacter();

                            Advanced = GUILayout.Toggle(Advanced, "Advanced Controls");
                            if (Advanced) AdvancedColor();

                            DrawOutfitsToChange();

                            DrawPartsToChange();

                            DrawColorsToChange();

                            DrawUniformToApply();

                            Outfits.EmblemFlag = GUILayout.Toggle(Outfits.EmblemFlag, "Change Emblem");

                            if (!GUIChanged) GUI.enabled = false;
                            if (GUILayout.Button("Apply to Selected")) Utilities.ApplySettings(Utilities.Apply.Card);
                            if (GUILayout.Button("Apply to Class")) Utilities.ApplySettings(Utilities.Apply.Class);
                            if (GUILayout.Button("Apply to All")) Utilities.ApplySettings(Utilities.Apply.All);
                        }
                        GUILayout.EndVertical();
                        GUILayout.Space(24);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            if (GUI.changed) GUIChanged = true;
        }
    }
}