using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using static Skills;

namespace NhxExtraJewels.GemEffects
{
    public static class ExpertHarvester
    {
        [PublicAPI]
        public struct Config
        {
            [InverseMultiplicativePercentagePower] public float Power;
        }
    }
}
