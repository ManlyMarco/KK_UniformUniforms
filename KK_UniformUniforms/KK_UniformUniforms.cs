using ActionGame;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Manager;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = Manager.Scene;

namespace KK_UniformUniforms
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class KK_UniformUniforms : BaseUnityPlugin
    {
        public const string GUID = "com.cptgrey.bepinex.uniform";
        public const string PluginName = "KK Uniform Uniforms";
        public const string Version = "1.1";

        internal static new ManualLogSource Logger;
        
        private static bool Visible;

        private void Start()
        {
            Logger = base.Logger;

            // Set color parameters
            UI.PreviewTexture = new Texture2D(65, 65)
            {
                wrapMode = TextureWrapMode.Repeat
            };

            // Only run when inside the class roster scene to save perf
            SceneManager.sceneLoaded += (arg0, arg1) =>
            {
                if (arg0.name == "ClassRoomSelect") enabled = true;
            };
            SceneManager.sceneUnloaded += arg0 =>
            {
                if (arg0.name == "ClassRoomSelect") enabled = false;
            };
            enabled = false;
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
                    Visible = true;
                }
                else
                {
                    Visible = false;
                }
            }
            else
            {
                // Reset stored classroom scene and disable gui
                Utilities.ClassroomScene = null;
                Visible = false;
            }
        }

        private void OnGUI()
        {
            if (Visible)
                UI.DrawInterface();
        }
    }
}