using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;

namespace NhxExtraJewels.GemEffects
{
    public static class EitrFused
    {
        [PublicAPI]
        public struct Config
        {
            [InverseMultiplicativePercentagePowerAttribute] public float Power;
            [AdditivePowerAttribute] public float Cost;
        }
    }
}
