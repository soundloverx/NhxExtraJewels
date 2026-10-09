using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using NhxExtraJewels.EffectHelpers;
using UnityEngine;
using Logger = NhxExtraJewels.Common.ModLog;

namespace NhxExtraJewels.GemEffects
{
    public static class CombatSpirit
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
        }

        // Seconds from one spirit wolf to the next. Each wolf lives for the effect's power in seconds.
        private const float Cooldown = 120f;

        // The local player's wolf. Unity reports it as null once it died or despawned.
        private static GameObject wolf = null;
        private static float cooldown_until = 0f;

        // Player.OnTargeted runs on the client that owns the attacking monster, which can be another player's client.
        // It forwards the event to the targeted player's own client as this RPC, every half second while monsters are
        // on them, so each player spawns and owns their own wolf. EpicJewels used OnTargeted, so whichever client owned
        // the monsters spawned the wolf, and one client allowed only one wolf for all players near it.
        [HarmonyPatch(typeof(Player), nameof(Player.RPC_OnTargeted))]
        public static class CombatCompanion
        {
            private static void Postfix(Player __instance, bool sensed, bool alerted)
            {
                if (!sensed || !alerted || __instance != Player.m_localPlayer) { return; }
                float power = __instance.GetEffectPower<Config>("Combat Spirit").Power;
                if (power <= 0 || wolf != null || Time.time < cooldown_until) { return; }

                GameObject wolf_prefab = ZNetScene.instance.GetPrefab("Wolf");
                if (wolf_prefab == null) {
                    Logger.LogWarning("Combat Spirit could not resolve the 'Wolf' prefab, skipping spawn.");
                    return;
                }

                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                Vector3 player_location = __instance.gameObject.transform.position;
                player_location.x += 1f; // spawn next to the player
                wolf = Object.Instantiate(wolf_prefab, player_location, rotation);
                cooldown_until = Time.time + Cooldown;
                // Spirit look, faction, health and lifetime, on every client
                Summons.Register(wolf.GetComponent<Character>(), Summons.Kind.SpiritWolf, power);
            }
        }
    }
}
