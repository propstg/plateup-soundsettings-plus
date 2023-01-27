using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSettings {
    public class MainMenu<T> : KLMenu<T> {

        private static readonly List<float> volumeValues = new List<float> { 0f, 1f / 16f, 0.25f, 0.5f, 1.0f };
        private static readonly List<string> volumeLabels = new List<string> {
            "<size=1.7><voffset=-0.5><mspace=1em><sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> ",
            "<size=1.7><voffset=-0.5><mspace=1em><sprite name=\"pip_filled\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> ",
            "<size=1.7><voffset=-0.5><mspace=1em><sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_empty\"> <sprite name=\"pip_empty\"> ",
            "<size=1.7><voffset=-0.5><mspace=1em><sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_empty\"> ",
            "<size=1.7><voffset=-0.5><mspace=1em><sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> <sprite name=\"pip_filled\"> "
        };

        public MainMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int _) {
            addFloat("Dish Washer", volumeValues, volumeLabels, SoundPreferences.DishWasherVolume);
            addFloat("Microwave", volumeValues, volumeLabels, SoundPreferences.MicrowaveVolume);
            addFloat("Mess", volumeValues, volumeLabels, SoundPreferences.MessVolume);
            addFloat("Ding", volumeValues, volumeLabels, SoundPreferences.DingVolume);
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
}