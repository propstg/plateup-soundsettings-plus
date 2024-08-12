using HarmonyLib;
using Kitchen;
using Kitchen.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SoundSettings {

    public class MainMenu : Menu<MenuAction> {

        private static readonly List<float> volumeValues = new List<float> { 0f, 1f / 16f, 0.25f, 0.5f, 1.0f };
        private static readonly List<string> volumeLabels = Enumerable.Range(0, 5).Select(createNormalLabel).ToList();
        private static readonly List<float> extraVolumeValues = new List<float> { 0f, 1f / 16f, 0.25f, 0.5f, 1.0f, 1.5f, 2.0f };
        private static readonly List<string> extendedVolumeLabels = Enumerable.Range(0, 7).Select(createExtendedLabel).ToList();

        public MainMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int _) {
            addFloat("Dish Washer", volumeValues, volumeLabels, SoundPreferences.DishWasherVolume);
            addFloat("Microwave", volumeValues, volumeLabels, SoundPreferences.MicrowaveVolume);
            addFloat("Mess", volumeValues, volumeLabels, SoundPreferences.MessVolume);
            addFloat("Ding", volumeValues, volumeLabels, SoundPreferences.DingVolume);
            addFloat("Process Complete", volumeValues, volumeLabels, SoundPreferences.ProcessCompleteVolume);
            addFloat("Pickup/Drop", volumeValues, volumeLabels, SoundPreferences.PickupDropVolume);
            addFloat("Footsteps", extraVolumeValues, extendedVolumeLabels, SoundPreferences.FootstepsVolume);
            New<SpacerElement>();
            New<SpacerElement>();
            AddButton(Localisation["MENU_BACK_SETTINGS"], delegate { RequestPreviousMenu(); });
        }

        private void addFloat(string label, List<float> values, List<string> labels, Pref pref) {
            Option<float> option = new Option<float>(values, SoundPreferences.getFloat(pref), labels);
            AddLabel(label);
            AddSelect(option);
            option.OnChanged += delegate (object _, float value) {
                SoundPreferences.setFloat(pref, value);
            };
        }

        private static string createNormalLabel(int filledPips) {
            return "<size=1.7><voffset=-0.5><mspace=1em>" +
                string.Concat(Enumerable.Repeat(" <sprite name=\"pip_filled\">", filledPips)) +
                string.Concat(Enumerable.Repeat(" <sprite name=\"pip_empty\">", 4 - filledPips));
        }

        private static string createExtendedLabel(int filledPips) {
            int filledWhitePips = Mathf.Min(4, filledPips);
            int emptyWhitePips = Mathf.Max(0, 4 - filledPips);
            int filledRedPips = Mathf.Max(0, filledPips - filledWhitePips);
            int emptyRedPips = Mathf.Max(0, 2 - filledRedPips);

            return "<size=1.7><voffset=-0.5><mspace=1em>" +
                string.Concat(Enumerable.Repeat(" <sprite name=\"pip_filled\">", filledWhitePips)) +
                string.Concat(Enumerable.Repeat(" <sprite name=\"pip_empty\">", emptyWhitePips)) +
                string.Concat(Enumerable.Repeat(" <sprite color=#ff0000 name=\"pip_filled\">", filledRedPips)) +
                string.Concat(Enumerable.Repeat(" <sprite color=#ff0000 name=\"pip_empty\">", emptyRedPips));
        }
    }


    [HarmonyPatch(typeof(PlayerPauseView), "SetupMenus")]
    class PauseMenu_Patch {

        public static bool Prefix(PlayerPauseView __instance) {
            ModuleList moduleList = (ModuleList)__instance.GetType().GetField("ModuleList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            MethodInfo addMenu = __instance.GetType().GetMethod("AddMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            addMenu.Invoke(__instance, new object[] { typeof(MainMenu), new MainMenu(__instance.ButtonContainer, moduleList) });
            return true;
        }
    }


    [HarmonyPatch(typeof(AccessibilityMenu<MenuAction>), "Setup")]
    class AddMenuToPauseMenu {

        public static bool Prefix(AccessibilityMenu<MenuAction> __instance) {
            MethodInfo addSubmenu = __instance.GetType().GetMethod("AddSubmenuButton", BindingFlags.NonPublic | BindingFlags.Instance);
            addSubmenu.Invoke(__instance, new object[] { Mod.MOD_NAME, typeof(MainMenu), false });
            return true;
        }
    }
}