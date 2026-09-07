using HarmonyLib;
using Kitchen.Components;
using System.Reflection;
using UnityEngine;

namespace SoundSettings {

    [HarmonyPatch(typeof(HingeSoundSource), "Update")]
    public class HingeSoundSourcePatch {

        private static readonly MethodInfo SetVolumeMethod = AccessTools.Method(typeof(HingeSoundSource), "SetVolume");

        public static void Postfix(HingeSoundSource __instance, ref float ___Volume, ref HingeJoint ___Joint, ref float ___MinimumVelocity, ref float ___VolumeVariation) {
            var effectVolume = SoundPreferences.getFloat(SoundPreferences.DoorVolume);

            ___Volume = Mathf.Clamp((Mathf.Abs(___Joint.velocity) - ___MinimumVelocity) / 90f, 0.0f, effectVolume) * ___VolumeVariation;
            SetVolumeMethod.Invoke(__instance, null);
        }
    }
}