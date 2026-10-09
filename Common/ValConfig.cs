using BepInEx.Configuration;

namespace NhxExtraJewels.Common
{
    internal class ValConfig
    {
        public static ConfigFile cfg;
        public static ConfigEntry<bool> EnableItemTooltipDisplay;
        public static ConfigEntry<bool> EnableDebugMode;

        public ValConfig(ConfigFile cfgref) {
            // Init with the default plugin config file
            cfg = cfgref;
            cfg.SaveOnConfigSet = false;
            CreateConfigValues(cfgref);
        }

        private void CreateConfigValues(ConfigFile Config)
        {
            EnableItemTooltipDisplay = Config.Bind("Client config", "EnableItemTooltipDisplay", true,
               new ConfigDescription("Enables displaying some stat modifications on the item tooltip.",
               null));
            EnableDebugMode = Config.Bind("Client config", "EnableDebugMode", false,
                new ConfigDescription("Enables debug logging.",
                null));
            EnableDebugMode.SettingChanged += ModLog.EnableDebugLogging;
            ModLog.CheckEnableDebugLogging();
        }
    }
}
