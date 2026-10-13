using Jewelcrafting;
using System.Collections.Generic;
using UnityEngine;

namespace NhxExtraJewels
{
    internal static class GemResources
    {
        class GemDefinition {
            // Shown by Jewelcrafting's own generated texts ("{Type} Gemstone", "Perfect {Type}", ...) and used in its
            // "Drop chance for {Type} Gemstones" config key, which can't contain an apostrophe. Localizations/English.json
            // overrides the displayed names.
            public string Type { get; set; }
            // Internal id: prefab names (Uncut_{ColorName}_Stone, ...), localization keys and the gem names in ExtraJewelsConfig.yaml
            public string ColorName { get; set; }
            public Color Color { get; set; }
        }

        // Same order and effects as EpicJewels' gems, renamed and recolored. Jewelcrafting tints its gem templates
        // (and the world formations) with these colors; they sit between Jewelcrafting's own gem tints, which are
        // pure red, orange, yellow, green, cyan, blue, magenta, white and black.
        static readonly List<GemDefinition> GemDefinitions = new List<GemDefinition> {
            // EpicJewels' Jade
            new GemDefinition() { Type = "Peridot", ColorName = "peridot", Color = new Color(0.6f, 0.9f, 0.1f, 1f) },
            // EpicJewels' Amber
            new GemDefinition() { Type = "Tiger Eye", ColorName = "tigerseye", Color = new Color(0.65f, 0.4f, 0.12f, 1f) },
            // EpicJewels' Aquamarine
            new GemDefinition() { Type = "Turquoise", ColorName = "turquoise", Color = new Color(0.1f, 0.85f, 0.65f, 1f) },
            // EpicJewels' Garnet
            new GemDefinition() { Type = "Rhodolite", ColorName = "rhodolite", Color = new Color(0.85f, 0.1f, 0.4f, 1f) },
            // EpicJewels' Opal
            new GemDefinition() { Type = "Moonstone", ColorName = "moonstone", Color = new Color(0.65f, 0.75f, 0.95f, 1f) },
            // EpicJewels' Amethyst
            new GemDefinition() { Type = "Tanzanite", ColorName = "tanzanite", Color = new Color(0.45f, 0.25f, 0.95f, 1f) },
        };

        static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public static void AddGems()
        {
            foreach (GemDefinition gem in GemDefinitions) {
                // The same steps as API.AddGems(type, colorName, color), which doesn't return the formation
                API.AddShardFromTemplate(gem.Type, gem.ColorName, gem.Color);
                API.AddAndRegisterUncutFromTemplate(gem.Type, gem.ColorName, gem.Color);
                GameObject formation = API.AddDestructibleFromTemplate(gem.Type, gem.ColorName, gem.Color);
                API.AddTieredGemFromTemplate(gem.Type, gem.ColorName, gem.Color);
                TintFormationGlow(formation, gem.Color);
            }

            API.AddGemConfig(Plugin.LoadEmbeddedAssetToString("ExtraJewelsConfig.yaml"));
        }

        // Jewelcrafting sets both the color and the glow (_EmissionColor) of its gem items to the gem color, but on the
        // formation only the color. The formation keeps its template's gray glow, which washes it out to a much paler
        // shade than the gems it drops. Give it the same glow as the items.
        static void TintFormationGlow(GameObject formation, Color color)
        {
            foreach (MeshRenderer renderer in formation.transform.Find("collider").GetComponentsInChildren<MeshRenderer>()) {
                renderer.material.SetColor(EmissionColor, color);
            }
        }
    }
}
