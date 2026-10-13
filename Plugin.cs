using BepInEx;
using BepInEx.Logging;
using NhxExtraJewels.Common;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace NhxExtraJewels
{
    // Port of EpicJewels 1.1.2 (MidnightsFX, MIT-0) with renamed and recolored gems, see CLAUDE.md
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("org.bepinex.plugins.jewelcrafting", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    // Both register the same gem effects and patch the same methods, so running both would double every effect
    [BepInIncompatibility("MidnightsFX.EpicJewels")]
    internal class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "NhxExtraJewels";
        public const string PluginName = "NhxExtraJewels";
        public const string PluginVersion = "1.0.1";

        public static ManualLogSource Log;

        internal ValConfig cfg;
        internal static AssetBundle EmbeddedResourceBundle;
        internal static Harmony Harmony = new Harmony(PluginGUID);
        public static Material spiritCreature;

        public void Awake() {
            Log = this.Logger;
            cfg = new ValConfig(Config);
            AddLocalizations();
            // EpicJewels' original asset bundle; only its spirit wolf material is still used
            EmbeddedResourceBundle = LoadAssetBundle("epicjewels");
            ModLog.LogDebug("Logging embedded assets.");
            foreach (string asset_name in EmbeddedResourceBundle.GetAllAssetNames()) {
                ModLog.LogDebug(asset_name);
            }
            ModLog.LogInfo("Let the gems flow.");
            GemEffects.EffectList.AddGemEffects();
            GemResources.AddGems();

            spiritCreature = EmbeddedResourceBundle.LoadAsset<Material>("spirit_animal_mat.mat");

            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony.PatchAll(assembly);

            ValConfig.cfg.SaveOnConfigSet = true;
            ValConfig.cfg.Save();
        }

        public static AssetBundle LoadAssetBundle(string bundleName)
        {
            var resourceAssembly = typeof(Plugin).Assembly;
            string text = null;
            AssetBundle result;
            try
            {
                text = resourceAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(bundleName));
            }
            catch (Exception) {}
            if (text == null) {
                ModLog.LogError($"Could not find an embedded asset bundle matching '{bundleName}'.");
                return null;
            }
            using (Stream stream = resourceAssembly.GetManifestResourceStream(text))
            {
                result = AssetBundle.LoadFromStream(stream);
            }
            return result;
        }

        public static String LoadEmbeddedAssetToString(string assetName)
        {
            var resourceAssembly = typeof(Plugin).Assembly;
            string text = null;
            string result;
            try
            {
                text = resourceAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(assetName));
            } catch (Exception) { }
            if (text == null) { return null; }
            using (Stream stream = resourceAssembly.GetManifestResourceStream(text))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        private void AddLocalizations() {
            // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
            CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();
            // load all localization files within the Localizations directory, e.g. NhxExtraJewels.Localizations.English.json
            ModLog.LogInfo("Loading Localizations.");
            foreach (string embeddedResouce in typeof(Plugin).Assembly.GetManifestResourceNames()) {
                if (!embeddedResouce.Contains("Localizations")) { continue; }
                // Read the localization file
                ModLog.LogDebug($"Reading localization resource: {embeddedResouce}");
                string localization = ReadEmbeddedResourceFile(embeddedResouce);
                // comments in the localization are not valid JSON, so those need to be stripped
                string cleaned_localization = Regex.Replace(localization, @"\/\/.*", "");
                // Just the localization name
                var localization_name = embeddedResouce.Split('.');
                ModLog.LogDebug($"Adding localization: {localization_name[2]}");
                Localization.AddJsonFile(localization_name[2], cleaned_localization);
            }
        }

        /// <summary>
        /// This reads an embedded file resouce name, these are all resouces packed into the DLL
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal static string ReadEmbeddedResourceFile(string filename) {
            using (var stream = typeof(Plugin).Assembly.GetManifestResourceStream(filename)) {
                using (var reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
        }


        // TODO: Remove once blaxxun either sets scaling size for the synergies box or enables text fit
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Awake))]
        public static class EnableSynergyTextfit
        {
            [HarmonyPriority(Priority.Last)]
            public static void Postfix()
            {
                IEnumerable<GameObject> objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.StartsWith("JC_Synergies_Window"));
                ModLog.LogDebug($"Found {objects.Count()} Synergy panels to update.");
                foreach (GameObject go in objects)
                {
                    // don't break crap if we can't modify a textbox
                    try
                    {
                        ModLog.LogDebug($"Updating Synergy GO {go}");
                        go.transform.Find("Bkg/Left_Text/Left_Text_1").gameObject.GetComponent<Text>().resizeTextForBestFit = true;
                    }
                    catch (Exception) { }
                }
            }
        }

    }
}
