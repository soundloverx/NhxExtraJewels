using JetBrains.Annotations;
using Jewelcrafting;

namespace NhxExtraJewels.GemEffects
{
    public static class Spellsword
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
        }
    }
}
