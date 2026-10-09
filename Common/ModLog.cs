using BepInEx.Logging;
using NhxExtraJewels.Common;
using System;


namespace NhxExtraJewels.Common {
    internal class ModLog {
        public static LogLevel Level = LogLevel.Info;

        public static void EnableDebugLogging(object sender, EventArgs e) {
            if (ValConfig.EnableDebugMode.Value) {
                Level = LogLevel.Debug;
            } else {
                Level = LogLevel.Info;
            }
            // set log level
        }

        public static void CheckEnableDebugLogging() {
            if (ValConfig.EnableDebugMode.Value) {
                Level = LogLevel.Debug;
            } else {
                Level = LogLevel.Info;
            }
        }

        public static void LogDebug(string message) {
            if (Level >= LogLevel.Debug) {
                Plugin.Log.LogInfo("[DEBUG]" + message);
            }
        }
        public static void LogInfo(string message) {
            if (Level >= LogLevel.Info) {
                Plugin.Log.LogInfo(message);
            }
        }

        public static void LogWarning(string message) {
            if (Level >= LogLevel.Warning) {
                Plugin.Log.LogWarning(message);
            }
        }

        public static void LogError(string message) {
            if (Level >= LogLevel.Error) {
                Plugin.Log.LogError(message);
            }
        }
    }
}
