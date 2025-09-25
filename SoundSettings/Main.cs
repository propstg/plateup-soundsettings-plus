using HarmonyLib;
using KitchenMods;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoundSettings {

    public class Mod : IModInitializer {

        public const string MOD_ID = "blargle.SoundSettingsPlus";
        public const string MOD_NAME = "SoundSettings+";
        public static readonly string MOD_VERSION = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.ToString();
        public static bool registered = false;

        public void PostActivate(KitchenMods.Mod mod) {
            if (!registered) {
                Log($"v{MOD_VERSION} initialized");
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MOD_ID);
            }
        }

        public void PreInject() {
            if (!registered) {
                SoundPreferences.registerPreferences();
                registered = true;
            }
        }

        public void PostInject() {}

        public static void Log(object message, [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null) {
            Debug.Log($"[{MOD_ID}] [{caller}({callingFilePath}:{lineNumber})] {message}");
        }
    }
}