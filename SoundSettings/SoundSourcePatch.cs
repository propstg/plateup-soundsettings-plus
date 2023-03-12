using HarmonyLib;
using Kitchen;
using Kitchen.Components;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSettings {

    [HarmonyPatch(typeof(SoundSource), "Update")]
    public class SoundSourcePatch {

        private static readonly Dictionary<string, Pref> NAME_TO_PREFS = new Dictionary<string, Pref> {
            { "dishwasher", SoundPreferences.DishWasherVolume },
            { "Microwave_close", SoundPreferences.MicrowaveVolume },
            { "Microwave_open", SoundPreferences.MicrowaveVolume },
            { "Microwave_active", SoundPreferences.MicrowaveVolume },
            { "mess_created", SoundPreferences.MessVolume },
            { "process_complete_special", SoundPreferences.DingVolume },
            { "process_complete0", SoundPreferences.ProcessCompleteVolume },
            { "process_complete1", SoundPreferences.ProcessCompleteVolume },
            { "process_complete2", SoundPreferences.ProcessCompleteVolume },
            { "process_complete3", SoundPreferences.ProcessCompleteVolume },
            { "impactWood_light_004", SoundPreferences.PickupDropVolume },
            { "impactWood_heavy_003", SoundPreferences.PickupDropVolume },
            { "Footsteps_mixdown2", SoundPreferences.FootstepsVolume },
        };

        private static readonly List<Pref> SHOULD_SET_VOLUME_DIRECTLY = new List<Pref> {
            SoundPreferences.MessVolume,
            SoundPreferences.DingVolume,
            SoundPreferences.ProcessCompleteVolume,
            SoundPreferences.PickupDropVolume,
            SoundPreferences.FootstepsVolume,
        };

        private static List<string> seenNames = new List<string>();

        public static void Postfix(SoundSource __instance, ref AudioSource ___Audio, AudioClip ___Clip, ref float ___VolumeMultiplier, ref float ___TargetVolume, ref float ___Volume) {
            object foundPref = getPrefByName(___Clip?.name);
            if (___Clip != null && !seenNames.Contains(___Clip.name)) {
                seenNames.Add(___Clip.name);
                Debug.Log($"[{Mod.MOD_ID}] [NEW SOUND] [CLIP] '{___Clip.name}'");
            }

            if (___TargetVolume > 0 && (foundPref != null)) {
                float selectedVolume = SoundPreferences.getFloat((Pref) foundPref);
                if (shouldSetDirectly((Pref) foundPref)) {
                    ___Audio.volume = selectedVolume * ___VolumeMultiplier;
                } else {
                    ___TargetVolume = selectedVolume * ___VolumeMultiplier;
                }
            }
        }

        private static object getPrefByName(string name) {
            if (name != null && NAME_TO_PREFS.TryGetValue(name, out Pref value)) {
                return value;
            }

            return null;
        }

        private static bool shouldSetDirectly(Pref pref) {
            return SHOULD_SET_VOLUME_DIRECTLY.Contains(pref);
        }
    }
}