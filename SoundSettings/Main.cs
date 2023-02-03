using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using System.Reflection;
using UnityEngine;

namespace SoundSettings {

    public class Mod : BaseMod {

        public const string MOD_ID = "blargle.SoundSettingsPlus";
        public const string MOD_NAME = "SoundSettings+";
        public const string MOD_VERSION = "0.0.4";
        public const string MOD_AUTHOR = "blargle";

        public Mod() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, "1.1.3", Assembly.GetExecutingAssembly()) { }

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
}