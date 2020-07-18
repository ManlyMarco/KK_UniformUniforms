using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UniRx;

namespace KK_UniformUniforms
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class KK_UniformUniforms : BaseUnityPlugin
    {
        public const string GUID = "com.cptgrey.bepinex.uniform";
        public const string PluginName = "KK Uniform Uniforms";
        public const string PluginNameInternal = "KK_UniformUniforms";
        public const string Version = "1.0";

        internal static new ManualLogSource Logger;

        private void Awake()
        {
            Logger = base.Logger;
        }

        private void Start()
        {
            // Set color parameters
            UI.PreviewTexture = new Texture2D(65, 65)
            {
                wrapMode = TextureWrapMode.Repeat
            };
        }

        private void Update()
        {
            // Check if Scene is set to ClassRoomSelect
            if (Manager.Scene.Instance.NowSceneNames[0] == "ClassRoomSelect")
            {
                // Check if classroom scene is currently stored
                if (Utilities.ClassroomScene == null) Utilities.ClassroomScene = Singleton<ActionGame.ClassRoomSelectScene>.Instance;

                // Check if scene is visible, i.e. not currently loading a character
                if (Utilities.ClassroomScene != null && Utilities.ClassroomScene.classRoomList.isVisible)
                {
                    // Get PreviewClassData instance
                    Traverse enterPreview = Traverse.Create(Utilities.ClassroomScene.classRoomList).Field("enterPreview");
                    if (enterPreview.FieldExists())
                        Utilities.CurrClassData = enterPreview.GetValue<ReactiveProperty<ActionGame.PreviewClassData>>().Value;

                    // Get current school emblem
                    Outfits.EmblemID = Singleton<Manager.Game>.Instance.saveData.emblemID;

                    // Set GUI visible flag
                    UI.Visible = true;
                }
                else UI.Visible = false;
            }
            else
            {
                // Reset stored classroom scene and disable gui
                Utilities.ClassroomScene = null;
                UI.Visible = false;
            }
        }

        private void OnGUI()
        {
            if (UI.Visible)
            {
                GUILayout.Window(94761634, UI.GetWindowRect(), UI.DrawMainGUI, "Set School Uniforms");
            }
        }
    }
}
