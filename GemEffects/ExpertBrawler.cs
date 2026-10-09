using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using static Skills;

namespace NhxExtraJewels.GemEffects
{
    public static class ExpertBrawler
    {
        [PublicAPI]
        public struct Config
        {
            [InverseMultiplicativePercentagePower] public float Power;
        }
    }
}
