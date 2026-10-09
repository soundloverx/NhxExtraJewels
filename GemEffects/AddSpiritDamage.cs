using JetBrains.Annotations;
using Jewelcrafting;

namespace NhxExtraJewels.GemEffects
{
    public static class AddSpiritDamage
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
            [AdditivePowerAttribute] public float Chance;
        }
    }
}
