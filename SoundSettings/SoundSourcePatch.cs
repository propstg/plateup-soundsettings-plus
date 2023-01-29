using HarmonyLib;
using Kitchen;
using Kitchen.Components;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSettings {
    [HarmonyPatch(typeof(SoundSource), "Update")]
    public class SoundSourcePatch {

        private static List<string> seenNames = new List<string>();

        public static void Postfix(SoundSource __instance, ref AudioSource ___Audio, AudioClip ___Clip, ref float ___VolumeMultiplier, ref float ___TargetVolume, ref float ___Volume) {
            object foundPref = getPrefByPartialName(___Clip?.name);
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

        private static object getPrefByPartialName(string name) {
            if ("dishwasher".Equals(name)) {
                return SoundPreferences.DishWasherVolume;
            } else if ("Microwave_close".Equals(name)) {
                return SoundPreferences.MicrowaveVolume;
            } else if ("Microwave_open".Equals(name)) {
                return SoundPreferences.MicrowaveVolume;
            } else if ("Microwave_active".Equals(name)) {
                return SoundPreferences.MicrowaveVolume;
            } else if ("mess_created".Equals(name)) {
                return SoundPreferences.MessVolume;
            } else if ("process_complete_special".Equals(name)) {
                return SoundPreferences.DingVolume;
            } else if ("impactWood_light_004".Equals(name) || "impactWood_heavy_003".Equals(name)) {
                return SoundPreferences.PickupDropVolume;
            } else if ("Footsteps_mixdown2".Equals(name)) {
                return SoundPreferences.FootstepsVolume;
            }
            return null;
        }

        private static bool shouldSetDirectly(Pref pref) {
            return pref == SoundPreferences.MessVolume || pref == SoundPreferences.DingVolume || pref == SoundPreferences.PickupDropVolume || pref == SoundPreferences.FootstepsVolume;
        }
    }
}