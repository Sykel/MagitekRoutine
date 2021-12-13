using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using Magitek.Enumerations;
using Magitek.Extensions;
using Magitek.Models.Astrologian;
using Magitek.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Auras = Magitek.Utilities.Auras;

namespace Magitek.Logic.Astrologian
{
    internal static class Buff
    {
        public static async Task<bool> Swiftcast()
        {
            if (await Spells.Swiftcast.CastAura(Core.Me, Auras.Swiftcast))
            {
                return await Coroutine.Wait(15000, () => Core.Me.HasAura(Auras.Swiftcast, true, 7000));
            }

            return false;
        }

        public static async Task<bool> LucidDreaming()
        {
            if (!AstrologianSettings.Instance.LucidDreaming)
                return false;

            if (!Core.Me.InCombat)
                return false;

            if (Core.Me.CurrentManaPercent > AstrologianSettings.Instance.LucidDreamingManaPercent)
                return false;

            return await Spells.LucidDreaming.Cast(Core.Me);
        }

        private static async Task<bool> SwiftCastAspectedHelios()
        {
            if (!await Swiftcast())
                return false;

            while (Core.Me.HasAura(Auras.Swiftcast))
            {
                if (await Spells.AspectedHelios.HealAura(Core.Me, Auras.AspectedHelios, false))
                    return true;

                await Coroutine.Yield();
            }

            return false;
        }

        public static async Task<bool> Lightspeed()
        {
            if (!AstrologianSettings.Instance.Lightspeed)
                return false;

            if (!Core.Me.InCombat)
                return false;

            if (Combat.CombatTotalTimeLeft <= 25)
                return false;

            if (Spells.Lightspeed.Cooldown != TimeSpan.Zero)
                return false;

            if (Core.Me.CurrentManaPercent > AstrologianSettings.Instance.LightspeedManaPercent)
                return false;

            //I Can't get this to work for some reason.
            //Let's try it now that lightspeed is also under CombatBuffs
            if (!MovementManager.IsMoving
                && !AstrologianSettings.Instance.LightspeedWhileMoving)
                return false;

            if (Globals.InParty)
            {
                if (AstrologianSettings.Instance.LightspeedTankOnly)
                {
                    if (Group.CastableTanks.Any(r => r.CurrentHealthPercent >= AstrologianSettings.Instance.LightspeedHealthPercent))
                        return false;

                    return await Spells.Lightspeed.CastAura(Core.Me, Auras.Lightspeed);
                }
                if (Group.CastableAlliesWithin30.Any(r => r.CurrentHealthPercent >= AstrologianSettings.Instance.LightspeedHealthPercent))
                    return false;

                return await Spells.Lightspeed.CastAura(Core.Me, Auras.Lightspeed);
            }

            return await Spells.Lightspeed.CastAura(Core.Me, Auras.Lightspeed);
        }

        public static async Task<bool> Synastry()
        {
            if (!AstrologianSettings.Instance.Synastry)
                return false;

            if (!Core.Me.InCombat)
                return false;

            if (!Globals.PartyInCombat)
                return false;

            if (Casting.LastSpell == Spells.Synastry)
                return false;

            if (Core.Me.HasAura(Auras.SynastrySource))
                return false;

            if (Group.CastableAlliesWithin30.Count(r => r.CurrentHealthPercent <= AstrologianSettings.Instance.SynastryHealthPercent) < AstrologianSettings.Instance.SynastryAmountOfPeople)
                return false;

            GameObject target = AstrologianSettings.Instance.SynastryTankOnly
                ? Group.CastableTanks.FirstOrDefault(r => r.CurrentHealthPercent <= AstrologianSettings.Instance.SynastryHealthPercent
                && r.IsTank())
                : Group.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealthPercent <= AstrologianSettings.Instance.SynastryHealthPercent);

            if (target == null)
                return false;

            return await Spells.Synastry.CastAura(target, Auras.SynastryDestination);
        }

        public static async Task<bool> NeutralSect()
        {
            if (!AstrologianSettings.Instance.NeutralSect)
                return false;

            var neutral = Group.CastableAlliesWithin30.Count(r => r.CurrentHealth > 0
            && r.Distance(Core.Me) <= 15
            && r.CurrentHealthPercent <= AstrologianSettings.Instance.NeutralSectHealthPercent);

            if (neutral < AstrologianSettings.Instance.NeutralSectAllies)
                return false;

            return await Spells.NeutralSect.Cast(Core.Me);
        }

        
    }
}