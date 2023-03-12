using Kitchen;

namespace SoundSettings {

    public class SoundPreferences {

        public static readonly Pref DishWasherVolume = new Pref(Mod.MOD_ID, nameof(DishWasherVolume));
        public static readonly Pref MicrowaveVolume = new Pref(Mod.MOD_ID, nameof(MicrowaveVolume));
        public static readonly Pref MessVolume = new Pref(Mod.MOD_ID, nameof(MessVolume));
        public static readonly Pref DingVolume = new Pref(Mod.MOD_ID, nameof(DingVolume));
        public static readonly Pref PickupDropVolume = new Pref(Mod.MOD_ID, nameof(PickupDropVolume));
        public static readonly Pref FootstepsVolume = new Pref(Mod.MOD_ID, nameof(FootstepsVolume));
        public static readonly Pref ProcessCompleteVolume = new Pref(Mod.MOD_ID, nameof(ProcessCompleteVolume));

        public static void registerPreferences() {
            Preferences.AddPreference<float>(new FloatPreference(DishWasherVolume, 0.25f));
            Preferences.AddPreference<float>(new FloatPreference(MicrowaveVolume, 0.25f));
            Preferences.AddPreference<float>(new FloatPreference(MessVolume, 1.0f));
            Preferences.AddPreference<float>(new FloatPreference(DingVolume, 1.0f));
            Preferences.AddPreference<float>(new FloatPreference(PickupDropVolume, 1.0f));
            Preferences.AddPreference<float>(new FloatPreference(FootstepsVolume, 1.0f));
            Preferences.AddPreference<float>(new FloatPreference(ProcessCompleteVolume, 1.0f));
            Preferences.Load();
        }

        public static float getFloat(Pref pref) {
            return Preferences.Get<float>(pref);
        }

        public static void setFloat(Pref pref, float value) {
            Preferences.Set<float>(pref, value);
        }
    }
}