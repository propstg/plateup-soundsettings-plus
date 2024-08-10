using HarmonyLib;
using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using KitchenMods;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoundSettings {

    public class Mod : IModInitializer {

        public const string MOD_ID = "blargle.SoundSettingsPlus";
        public const string MOD_NAME = "SoundSettings+";
        public static readonly string MOD_VERSION = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.ToString();

        public void PostActivate(KitchenMods.Mod mod) {
            Log($"v{MOD_VERSION} initialized");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MOD_ID);
        }

        public void PreInject() {
            SoundPreferences.registerPreferences();
        }

        public void PostInject() {}

        public static void Log(object message, [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null) {
            Debug.Log($"[{MOD_ID}] [{caller}({callingFilePath}:{lineNumber})] {message}");
        }
    }
}