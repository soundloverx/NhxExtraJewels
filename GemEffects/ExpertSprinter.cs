using JetBrains.Annotations;
using Jewelcrafting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhxExtraJewels.GemEffects
{
    public static class ExpertSprinter
    {
        [PublicAPI]
        public struct Config
        {
            [InverseMultiplicativePercentagePower] public float Power;
        }
    }
}
