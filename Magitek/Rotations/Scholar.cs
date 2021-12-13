﻿using System.Linq;
using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Logic;
using Magitek.Logic.Scholar;
using Magitek.Models.Scholar;
using Magitek.Utilities;
using System.Threading.Tasks;

namespace Magitek.Rotations
{
    public static class Scholar
    {
        public static async Task<bool> Rest()
        {
            if (Core.Me.CurrentHealthPercent > 70 || Core.Me.ClassLevel < 4)
                return false;

            await Spells.Physick.Heal(Core.Me);
            return true;
        }

        public static async Task<bool> PreCombatBuff()
        {
            if (Core.Me.IsCasting)
                return true;

            await Casting.CheckForSuccessfulCast();

            if (Core.Me.IsMounted)
                return false;

            if (CustomOpenerLogic.InOpener) return false;

            if (await Buff.SummonPet()) return true;
            return await Buff.Aetherflow();
        }

        public static async Task<bool> Pull()
        {
            if (BotManager.Current.IsAutonomous)
            {
                if (Core.Me.HasTarget)
                {
                    Movement.NavigateToUnitLos(Core.Me.CurrentTarget, 20);
                }
            }
            else
            {
                if (!ScholarSettings.Instance.DoDamage)
                    return false;
            }

            return await Heal();
        }
        public static async Task<bool> Heal()
        {
            if (Core.Me.IsMounted)
                return true;

            if (await Casting.TrackSpellCast()) return true;
            
            await Casting.CheckForSuccessfulCast();

            Casting.DoHealthChecks = false;

            if (await GambitLogic.Gambit()) return true;

            if (CustomOpenerLogic.InOpener) return false;
            
            if (await Logic.Scholar.Heal.Resurrection()) return true;

            // Scalebound Extreme Rathalos
            if (Core.Me.HasAura(1495))
            {
                return await Dispel.Execute();
            }

            #region Pre-Healing Stuff

            if (await Logic.Scholar.Heal.ForceWhispDawn()) return true;
            if (await Logic.Scholar.Heal.ForceAdlo()) return true;
            if (await Logic.Scholar.Heal.ForceIndom()) return true;
            if (await Logic.Scholar.Heal.ForceExcog()) return true;
            
            if (await Dispel.Execute()) return true;

            #endregion

            if (Weaving.GetCurrentWeavingCounter() < 2 && Spells.Ruin.Cooldown.TotalMilliseconds >
                650 + BaseSettings.Instance.UserLatencyOffset)
            {
                if (Core.Me.Pet != null && Core.Me.InCombat)
                {
                    if (await Logic.Scholar.Heal.FeyBlessing()) return true;
                    if (await Logic.Scholar.Heal.WhisperingDawn()) return true;
                    if (await Logic.Scholar.Heal.FeyIllumination()) return true;
                    if (await Logic.Scholar.Heal.SummonSeraph()) return true;
                    if (await Logic.Scholar.Heal.Consolation()) return true;
                    if (await Buff.Aetherpact()) return true;
                    if (await Buff.BreakAetherpact()) return true;
                }

                if (Globals.InParty)
                {
                    if (await Logic.Scholar.Heal.Indomitability()) return true;
                    if (await Logic.Scholar.Heal.SacredSoil()) return true;
                    if (await Buff.DeploymentTactics()) return true;
                }

                if (await Logic.Scholar.Heal.Excogitation()) return true;
                if (await Logic.Scholar.Heal.Lustrate()) return true;

                if (await Buff.Aetherflow()) return true;
                if (await Buff.ChainStrategem()) return true;
                if (await Buff.LucidDreaming()) return true;
            }

            if (Globals.InParty)
            {
                if (await Logic.Scholar.Heal.EmergencyTacticsSuccor()) return true;
                if (await Logic.Scholar.Heal.Succor()) return true;
            }

            if (await Logic.Scholar.Heal.EmergencyTacticsAdloquium()) return true;
            if (await Logic.Scholar.Heal.Adloquium()) return true;
            if (await Logic.Scholar.Heal.Physick()) return true;

            if (await Buff.SummonPet()) return true;

            if (Utilities.Combat.Enemies.Count > ScholarSettings.Instance.StopDamageWhenMoreThanEnemies)
                return true;

            if (Globals.InParty)
            {
                if (!ScholarSettings.Instance.DoDamage)
                    return true;

                if (Core.Me.CurrentManaPercent < ScholarSettings.Instance.MinimumManaPercent)
                    return true;
            }

            return await Combat();
        }
        public static async Task<bool> CombatBuff()
        {
            return false;
        }
        public static async Task<bool> Combat()
        {
            if (BotManager.Current.IsAutonomous)
            {
                if (Core.Me.HasTarget)
                {
                    Movement.NavigateToUnitLos(Core.Me.CurrentTarget, 20);
                }
            }

            if (await Casting.TrackSpellCast()) return true;
            await Casting.CheckForSuccessfulCast();

            if (Core.Me.CurrentManaPercent <= ScholarSettings.Instance.MinimumManaPercent)
                return false;

            if (await Buff.SummonPet()) return true;

            if (Utilities.Combat.Enemies.Count > ScholarSettings.Instance.StopDamageWhenMoreThanEnemies)
                return true;

            if (Globals.InParty)
            {
                if (!ScholarSettings.Instance.DoDamage)
                    return true;

                if (Core.Me.CurrentManaPercent < ScholarSettings.Instance.MinimumManaPercent)
                    return true;

                if (Group.CastableAlliesWithin30.Any(c => c.IsAlive && c.CurrentHealthPercent < ScholarSettings.Instance.DamageOnlyIfAboveHealthPercent))
                    return true;
            }

            if (!Core.Me.HasTarget || !Core.Me.CurrentTarget.ThoroughCanAttack())
                return false;

            if (await SingleTarget.BioMultipleTargets()) return true;

            if (await Aoe.ArtOfWar()) return true;

            if (Core.Me.CurrentTarget.HasAnyAura(Auras.Invincibility))
                return false;

            if (await SingleTarget.Bio()) return true;
            if (await SingleTarget.Ruin2()) return true;
            if (await SingleTarget.EnergyDrain2()) return true;
            return await SingleTarget.Broil();
        }
        public static async Task<bool> PvP()
        {
            return false;
        }
    }
}
