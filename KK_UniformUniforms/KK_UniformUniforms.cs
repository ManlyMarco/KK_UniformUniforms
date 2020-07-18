using ActionGame;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Manager;
using UniRx;
using UnityEngine;

namespace KK_UniformUniforms
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class KK_UniformUniforms : BaseUnityPlugin
    {
        public const string GUID = "com.cptgrey.bepinex.uniform";
        public const string PluginName = "KK Uniform Uniforms";
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
            if (Scene.Instance.NowSceneNames[0] == "ClassRoomSelect")
            {
                // Check if classroom scene is currently stored
                if (Utilities.ClassroomScene == null)
                    Utilities.ClassroomScene = Singleton<ClassRoomSelectScene>.Instance;

                // Check if scene is visible, i.e. not currently loading a character
                if (Utilities.ClassroomScene != null && Utilities.ClassroomScene.classRoomList.isVisible)
                {
                    // Get PreviewClassData instance
                    var enterPreview = Traverse.Create(Utilities.ClassroomScene.classRoomList).Field("enterPreview");
                    if (enterPreview.FieldExists())
                        Utilities.CurrClassData = enterPreview.GetValue<ReactiveProperty<PreviewClassData>>().Value;

                    // Get current school emblem
                    Outfits.EmblemID = Singleton<Game>.Instance.saveData.emblemID;

                    // Set GUI visible flag
                    UI.Visible = true;
                }
                else
                {
                    UI.Visible = false;
                }
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
            if (UI.Visible) GUILayout.Window(94761634, UI.GetWindowRect(), UI.DrawMainGUI, "Set School Uniforms");
        }
    }
}