using JetBrains.Annotations;
using Jewelcrafting;

namespace NhxExtraJewels.GemEffects
{
    public static class Inferno
    {
        [PublicAPI]
        public struct Config
        {
            [MultiplicativePercentagePowerAttribute] public float Power;
            [MultiplicativePercentagePowerAttribute] public float Chance;
        }
    }
}
