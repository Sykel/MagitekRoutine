using System;
using System.Collections.Generic;

namespace Magitek.Utilities.Routines
{
    internal static class DarkKnight
    {
        public static int PullUnleash;

        public static readonly List<uint> Defensives = new List<uint>()
        {
            Auras.Foresight,
            Auras.LivingDead,
            Auras.DarkDance,
            Auras.Shadowskin,
            Auras.ShadowWall
        };

        public static bool OnGcd => Spells.HardSlash.Cooldown > TimeSpan.FromMilliseconds(500);
    }
}