using HarmonyLib;
using Kitchen;
using Kitchen.Components;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SoundSettings {

    public class Mod : BaseMod {

        public const string MOD_ID = "blargle.SoundSettingsPlus";
        public const string MOD_NAME = "SoundSettings+";
        public const string MOD_VERSION = "0.0.1";
        public const string MOD_AUTHOR = "blargle";

        public Mod() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, "1.1.2", Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise() {
            SoundPreferences.registerPreferences();
            initMenus();
        }

        public static void Log(object message) {
            Debug.Log($"[{MOD_ID}] {message}");
        }

        private void initMenus() {
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(MainMenu<PauseMenuAction>), typeof(PauseMenuAction));
            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (s, args) => {
                args.Menus.Add(typeof(MainMenu<PauseMenuAction>), new MainMenu<PauseMenuAction>(args.Container, args.Module_list));
            };
        }
    }

    public class SoundPreferences {
        public static readonly Pref DishWasherVolume = new Pref(Mod.MOD_ID, nameof(DishWasherVolume));
        public static readonly Pref MicrowaveVolume = new Pref(Mod.MOD_ID, nameof(MicrowaveVolume));

        public static void registerPreferences() {
            Preferences.AddPreference<float>(new Kitchen.FloatPreference(DishWasherVolume, 0.25f));
            Preferences.AddPreference<float>(new Kitchen.FloatPreference(MicrowaveVolume, 0.25f));
            Preferences.Load();
        }

        public static float getFloat(Pref pref) {
            return Preferences.Get<float>(pref);
        }

        public static void setFloat(Pref pref, float value) {
            Preferences.Set<float>(pref, value);
        }
    }

    public class MainMenu<T> : KLMenu<T> {

        private static readonly List<float> volumeValues = new List<float> { 0f, 1f / 16f, 0.25f, 0.5f, 1.0f };
        private static readonly List<string> volumeLabels = new List<string> {
            "<sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> ",
            "<sprite name=\"pip_filled\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> ",
            "<sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> ",
            "<sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_empty\"> ",
            "<sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> "
        };

        public MainMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int _) {
            addFloat("Dish Washer", volumeValues, volumeLabels, SoundPreferences.DishWasherVolume);
            addFloat("Microwave", volumeValues, volumeLabels, SoundPreferences.MicrowaveVolume);
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
    }

    [HarmonyPatch(typeof(SoundSource), "Update")]
    public class SoundSourcePatch {

        public static void Postfix(SoundSource __instance, AudioSource ___Audio, ref float ___VolumeMultiplier, float ___TargetVolume) {
            bool isDishwasher = ___Audio.name.StartsWith("Dish Washer");
            bool isMicrowave = ___Audio.name.StartsWith("Microwave");
            if (___TargetVolume > 0 && (isDishwasher || isMicrowave)) {
                float newVolume = SoundPreferences.getFloat(isDishwasher ? SoundPreferences.DishWasherVolume : SoundPreferences.MicrowaveVolume);
                if (___VolumeMultiplier != newVolume) {
                    ___VolumeMultiplier = newVolume;
                    MethodInfo methodInfo = ReflectionUtils.GetMethod<SoundSource>("SetVolume", BindingFlags.NonPublic | BindingFlags.Instance);
                    methodInfo.Invoke(__instance, null);
                }
            }
        }
    }
}