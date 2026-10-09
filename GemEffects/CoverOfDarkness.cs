using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using NhxExtraJewels.EffectHelpers;
using System.Collections.Generic;
using UnityEngine;
using Logger = NhxExtraJewels.Common.ModLog;

namespace NhxExtraJewels.GemEffects
{
    public static class CoverOfDarkness
    {
        [PublicAPI]
        public struct Config
        {
            [InverseMultiplicativePercentagePower] public float Power;
            [InverseMultiplicativePercentagePower] public float Chance;
            [AdditivePowerAttribute] public float MaxCount;
        }

        public static List<ZDOID> spawnedBats = new List<ZDOID>();
        private static GameObject bat = null;

        [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
        public static class SummonBatHelpers
        {
            private static void Prefix(HitData hit)
            {
                if (hit.GetAttacker() is Player)
                {
                    Player player = hit.GetAttacker() as Player;
                    if (player.GetEffectPower<Config>("Cover of Darkness").Power > 0)
                    {
                        if (Random.value < (player.GetEffectPower<Config>("Cover of Darkness").Chance/ 100))
                        {
                            // Logger.LogDebug($"Cover of darkness triggered darkness power: {player.GetEffectPower<Config>("Cover of Darkness").Power}");
                            int bat_stars = 1; // 1 = 0 stars, 2 = 1 star etc
                            switch (player.GetEffectPower<Config>("Cover of Darkness").Power)
                            {
                                // Ordering here increases stars based on the first power level curve point
                                case float power when power > 70f:
                                    bat_stars = 4;
                                    break;
                                case float power when power > 50f:
                                    bat_stars = 3;
                                    break;
                                case float power when power > 30f:
                                    bat_stars = 2;
                                    break;
                            }
                            if (spawnedBats.Count >= player.GetEffectPower<Config>("Cover of Darkness").MaxCount) {
                                // Logger.LogDebug($"Cover of darkness max ({player.GetEffectPower<Config>("Cover of Darkness").MaxCount}) triggered, checking spawned bats liveliness.");
                                List<ZDOID> alive_bats = new List<ZDOID>();
                                foreach(ZDOID bat in spawnedBats)
                                {
                                    GameObject bat_exists = ZNetScene.instance.FindInstance(bat);
                                    // Logger.LogDebug($"Cover of darkness checking bat dead: {(bat_exists == null)}");
                                    if (bat_exists != null && !alive_bats.Contains(bat)) {
                                        alive_bats.Add(bat);
                                    }
                                }
                                spawnedBats = alive_bats;
                                if (spawnedBats.Count >= player.GetEffectPower<Config>("Cover of Darkness").MaxCount) {
                                    return; 
                                }
                            }

                            // Spawn an extra bat above 90 power
                            if (player.GetEffectPower<Config>("Cover of Darkness").Power > 90)
                            {
                                SpawnBat(player, bat_stars);
                                SpawnBat(player, bat_stars);
                            } else {
                                SpawnBat(player, bat_stars);
                            }
                            
                        }
                    }
                }
            }
        }

        static void SpawnBat( Player player, int stars = 1)
        {
            // Set the reference to a bat
            if (bat == null)
            {
                bat = ZNetScene.instance.GetPrefab("Bat");
                if (bat == null) {
                    Logger.LogWarning("Cover of Darkness could not resolve the 'Bat' prefab, skipping spawn.");
                    return;
                }
            }

            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Vector3 player_location = player.gameObject.transform.position;
            player_location.y += 1f; // spawn just above the player
            GameObject creature = Object.Instantiate(bat, player_location, rotation);
            Character creature_character = creature.GetComponent<Character>();
            MonsterAI creature_AI = creature.GetComponent<MonsterAI>();
            if (creature_AI == null) {
                // This creatures faction can't be set so destroy it
                ZNetScene.instance.Destroy(creature);
                return;
            }
            // Increase the bats level based on how strong the darkness is. SetLevel stores it in the ZDO and scales the
            // health; EpicJewels set m_level, which only changed the summoner's copy and left the health at 0 stars.
            creature_character.SetLevel(stars);
            // Set the creatures faction to the player
            creature_AI.MakeTame();
            // No drops, the Tameable component and the lifetime, on every client
            Summons.Register(creature_character, Summons.Kind.Bat, player.GetEffectPower<Config>("Cover of Darkness").Power);
            spawnedBats.Add(creature_character.GetZDOID());
        }
    }

}
