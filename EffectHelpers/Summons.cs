using HarmonyLib;
using System;
using UnityEngine;

namespace NhxExtraJewels.EffectHelpers
{
    // The gem summons (Combat Spirit's wolf, Cover of Darkness' bats) are ordinary networked creatures, and changes to
    // their components and fields don't reach other clients. EpicJewels only set them up on the summoner's client, so
    // other players saw a plain wild wolf, and a summon that changed owner before its timer ran out (for example when
    // the summoner logged out) stayed in the world for good. Here the summon's kind and expiry time go into its ZDO,
    // and every client sets the creature up from that when it loads.
    internal static class Summons
    {
        internal enum Kind
        {
            None = 0,
            SpiritWolf = 1,
            Bat = 2,
        }

        private static readonly int KindKey = "NhxExtraJewels_summon".GetStableHashCode();
        private static readonly int ExpiresKey = "NhxExtraJewels_summon_expires".GetStableHashCode();

        // Lox health, but not lox resistances
        private const float SpiritWolfHealth = 1000f;

        // Marks a creature the local player just spawned as a summon that dies after lifetime seconds
        internal static void Register(Character character, Kind kind, float lifetime)
        {
            ZDO zdo = character.m_nview.GetZDO();
            zdo.Set(KindKey, (int)kind);
            zdo.Set(ExpiresKey, ZNet.instance.GetTime().AddSeconds(lifetime).Ticks);
            // Character.Awake already ran during Instantiate, before the ZDO had these values
            Setup(character, kind);
        }

        internal static DateTime GetExpiry(ZDO zdo)
        {
            return new DateTime(zdo.GetLong(ExpiresKey));
        }

        private static void Setup(Character character, Kind kind)
        {
            switch (kind) {
                case Kind.SpiritWolf:
                    SetupSpiritWolf(character);
                    break;
                case Kind.Bat:
                    SetupBat(character);
                    break;
            }
            if (character.GetComponent<SummonLifetime>() == null) {
                character.gameObject.AddComponent<SummonLifetime>();
            }
        }

        private static void SetupSpiritWolf(Character wolf)
        {
            SkinnedMeshRenderer wolf_renderer = wolf.GetComponentInChildren<SkinnedMeshRenderer>();
            if (wolf_renderer != null && Plugin.spiritCreature != null) { wolf_renderer.material = Plugin.spiritCreature; }
            DestroyComponent<CharacterDrop>(wolf);
            DestroyComponent<Tameable>(wolf);
            MonsterAI creature_ai = wolf.GetComponent<MonsterAI>();
            if (creature_ai != null) { creature_ai.m_attackPlayerObjects = false; }
            for (int i = 0; i < wolf.m_deathEffects.m_effectPrefabs.Length && i < 2; i++) {
                wolf.m_deathEffects.m_effectPrefabs[i].m_enabled = false;
            }
            wolf.m_faction = Character.Faction.Players;
            wolf.gameObject.name = "Spirit Wolf";
            wolf.m_health = SpiritWolfHealth;
            // Character.Awake stores the max health in the ZDO on the owner, from the wolf's own m_health. EpicJewels
            // changed m_health only after that, so the spirit wolf kept normal wolf health.
            if (wolf.m_nview.IsOwner()) { wolf.SetMaxHealth(wolf.GetMaxHealthBase() * wolf.GetLevel()); }
        }

        private static void SetupBat(Character bat)
        {
            DestroyComponent<CharacterDrop>(bat);
            // As in EpicJewels. The bat is tamed through its ZDO (MonsterAI.MakeTame), this only adds the component.
            if (bat.GetComponent<Tameable>() == null) { bat.gameObject.AddComponent<Tameable>(); }
        }

        private static void DestroyComponent<T>(Character character) where T : Component
        {
            T component = character.GetComponent<T>();
            if (component != null) { UnityEngine.Object.Destroy(component); }
        }

        [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
        private static class SetupSummonOnLoad
        {
            private static void Postfix(Character __instance)
            {
                ZDO zdo = __instance.m_nview != null ? __instance.m_nview.GetZDO() : null;
                if (zdo == null) { return; }
                Kind kind = (Kind)zdo.GetInt(KindKey);
                if (kind != Kind.None) { Setup(__instance, kind); }
            }
        }
    }

    // Kills the summon when it expires, on whichever client owns it at that moment
    internal class SummonLifetime : MonoBehaviour
    {
        // A summon found further past its expiry than this expired while no one was near, and is removed without dying
        private const double OverdueSeconds = 5;

        private Character m_character;

        private void Awake()
        {
            m_character = GetComponent<Character>();
            InvokeRepeating(nameof(CheckExpiry), 1f, 1f);
        }

        private void CheckExpiry()
        {
            if (m_character == null || ZNet.instance == null) { return; }
            ZNetView nview = m_character.m_nview;
            if (nview == null || !nview.IsValid() || !nview.IsOwner() || m_character.IsDead()) { return; }
            double overdue = (ZNet.instance.GetTime() - Summons.GetExpiry(nview.GetZDO())).TotalSeconds;
            if (overdue < 0) { return; }
            if (overdue > OverdueSeconds) {
                ZNetScene.instance.Destroy(gameObject);
                return;
            }
            // The same lethal hit as vanilla's CharacterTimedDestruction
            m_character.ApplyDamage(new HitData { m_damage = { m_damage = 99999f }, m_point = transform.position }, false, true);
        }
    }
}
