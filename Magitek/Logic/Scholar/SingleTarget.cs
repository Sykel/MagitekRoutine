using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using Magitek.Extensions;
using Magitek.Models.Account;
using Magitek.Models.Scholar;
using Magitek.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Auras = Magitek.Utilities.Auras;

namespace Magitek.Logic.Scholar
{
    internal static class SingleTarget
    {
        public static async Task<bool> Broil()
        {
            if (!ScholarSettings.Instance.RuinBroil)
                return false;

            return await Spells.Ruin.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> Ruin2()
        {
            if (!ScholarSettings.Instance.Ruin2)
                return false;

            if (Core.Me.HasAura(Auras.Swiftcast) || Core.Me.HasAura("Lost Chainspell"))
                return false;

            if (NeedWeavingOpportunity())
                return await Spells.Ruin2.Cast(Core.Me.CurrentTarget);

            if (!MovementManager.IsMoving)
                return false;

            return await Spells.Ruin2.Cast(Core.Me.CurrentTarget);
        }

        private static bool NeedWeavingOpportunity()
        {
            if (ActionResourceManager.Scholar.Aetherflow == 3 && ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 9000 && Spells.EnergyDrain2.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (ActionResourceManager.Scholar.Aetherflow == 2 && ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 6000 && Spells.EnergyDrain2.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (ActionResourceManager.Scholar.Aetherflow == 1 && ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 3000 && Spells.EnergyDrain2.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (ActionResourceManager.Scholar.Aetherflow == 3 && ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 9000 && Spells.EnergyDrain2.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (ActionResourceManager.Scholar.Aetherflow == 2 && ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 6000 && Spells.EnergyDrain2.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (ActionResourceManager.Scholar.Aetherflow == 1 && ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 3000 && Spells.EnergyDrain2.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (ActionResourceManager.Scholar.Aetherflow == 0 && 
                (ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 1500 || 
                (ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 1500)))
                return true;
            if (Logic.Scholar.Heal.ShouldExcogitation() && ActionManager.HasSpell(Spells.Excogitation.Id) && Spells.Excogitation.Cooldown.TotalMilliseconds < 1500 && ActionResourceManager.Scholar.Aetherflow > 0)
                return true;
            if (Logic.Scholar.Heal.ShouldLustrate() && ActionManager.HasSpell(Spells.Lustrate.Id) && ActionResourceManager.Scholar.Aetherflow > 0)
                return true;
            if (Logic.Scholar.Heal.ShouldIndomitability() && ActionManager.HasSpell(Spells.Indomitability.Id) && ActionResourceManager.Scholar.Aetherflow > 0)
                return true;
            if (Logic.Scholar.Heal.ShouldFeyBlessing() && ActionManager.HasSpell(Spells.FeyBlessing.Id) && Spells.FeyBlessing.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Heal.ShouldWhisperingDawn() && ActionManager.HasSpell(Spells.WhisperingDawn.Id) && Spells.WhisperingDawn.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Heal.ShouldFeyIllumination() && ActionManager.HasSpell(Spells.FeyIllumination.Id) && Spells.FeyIllumination.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Heal.ShouldSummonSeraph() && ActionManager.HasSpell(Spells.SummonSeraph.Id) && Spells.SummonSeraph.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Heal.ShouldConsolation() && ActionManager.HasSpell(Spells.Consolation.Id) && (int)PetManager.ActivePetType == 15 && Spells.Consolation.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Buff.ShouldChainStratagem() && ActionManager.HasSpell(Spells.ChainStrategem.Id) && Spells.ChainStrategem.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Buff.ShouldLucidDreaming())
                return true;
            if (Logic.Scholar.Buff.ShouldDeploymentTactics() && ActionManager.HasSpell(Spells.DeploymentTactics.Id) && Spells.DeploymentTactics.Cooldown.TotalMilliseconds < 1500)
                return true;
            if (Logic.Scholar.Buff.ShouldAetherpact())
                return true;
            if (Logic.Scholar.Buff.ShouldBreakAetherpact())
                return true;
            return false;
        }

        public static async Task<bool> BioMultipleTargets()
        {
            if (!ScholarSettings.Instance.Bio)
                return false;

            if (!ScholarSettings.Instance.BioMultipleTargets)
                return false;

            if (Combat.Enemies.Count(HasMyBio) >= ScholarSettings.Instance.BioTargetLimit)
                return false;

            var bioTarget = Combat.Enemies.FirstOrDefault(NeedsBio);

            if (bioTarget == null)
                return false;

            return await Spells.Bio.Cast(bioTarget);

            bool HasMyBio(BattleCharacter unit) {
                if (unit == null) return false;

                if (Core.Me.ClassLevel < 26)
                    return unit.HasAura(Auras.Bio, true, ScholarSettings.Instance.BioRefreshSeconds * 1000);

                if (Core.Me.ClassLevel < 72)
                    return unit.HasAura(Auras.Bio2, true, ScholarSettings.Instance.BioRefreshSeconds * 1000);

                return unit.HasAura(Auras.Biolysis, true, ScholarSettings.Instance.BioRefreshSeconds * 1000);
            }

            bool NeedsBio(BattleCharacter unit)
            {
                if (!CanBio(unit))
                    return false;

                if (!unit.InLineOfSight()) return false;

                if (unit.HasAnyAura(Auras.Invincibility))
                    return false;

                if (Core.Me.ClassLevel < 26)
                    return !unit.HasAura(Auras.Bio, true, ScholarSettings.Instance.BioRefreshSeconds * 1000);

                if (Core.Me.ClassLevel < 72)
                    return !unit.HasAura(Auras.Bio2, true, ScholarSettings.Instance.BioRefreshSeconds * 1000);

                return !unit.HasAura(Auras.Biolysis, true, ScholarSettings.Instance.BioRefreshSeconds * 1000);
            }
            
            bool CanBio(GameObject unit)
            {
                if (!ScholarSettings.Instance.BioUseTimeTillDeath)
                    return true;

                return unit.CombatTimeLeft() >= ScholarSettings.Instance.BioDontIfEnemyDyingWithinSeconds;
            }
        }

        public static async Task<bool> Bio()
        {
            if (!ScholarSettings.Instance.Bio)
                return false;

            if (Core.Me.CurrentTarget.HasAnyAura(BioAuras, true, 4000))
                return false;

            return await Spells.Bio.Cast(Core.Me.CurrentTarget);
        }

        private static readonly uint[] BioAuras =
        {
            Auras.Bio,
            Auras.Bio2,
            Auras.Biolysis
        };

        public static async Task<bool> EnergyDrain2()
        {
            if (!ScholarSettings.Instance.EnergyDrain)
                return false;

            //if (Core.Me.CurrentManaPercent > ScholarSettings.Instance.EnergyDrainManaPercent || Core.Me.CurrentManaPercent > 100)
            //    return false;

            if (!Core.Me.HasAetherflow())
                return false;

            if (Weaving.GetCurrentWeavingCounter() < 2 && Spells.Ruin.Cooldown.TotalMilliseconds >
               650 + BaseSettings.Instance.UserLatencyOffset)
            {
                if (ActionResourceManager.Scholar.Aetherflow == 3 && ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 9000)
                    return await Spells.EnergyDrain2.Cast(Core.Me.CurrentTarget);
                if (ActionResourceManager.Scholar.Aetherflow == 2 && ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 6000)
                    return await Spells.EnergyDrain2.Cast(Core.Me.CurrentTarget);
                if (ActionResourceManager.Scholar.Aetherflow == 1 && ActionManager.HasSpell(Spells.Aetherflow.Id) && Spells.Aetherflow.Cooldown.TotalMilliseconds < 3000)
                    return await Spells.EnergyDrain2.Cast(Core.Me.CurrentTarget);
                if (ActionResourceManager.Scholar.Aetherflow == 3 && ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 9000)
                    return await Spells.EnergyDrain2.Cast(Core.Me.CurrentTarget);
                if (ActionResourceManager.Scholar.Aetherflow == 2 && ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 6000)
                    return await Spells.EnergyDrain2.Cast(Core.Me.CurrentTarget);
                if (ActionResourceManager.Scholar.Aetherflow == 1 && ActionManager.HasSpell(Spells.Dissipation.Id) && Spells.Dissipation.Cooldown.TotalMilliseconds < 3000)
                    return await Spells.EnergyDrain2.Cast(Core.Me.CurrentTarget);

            }

            return false;
            //if (Casting.LastSpell != Spells.Biolysis || Casting.LastSpell != Spells.ArtOfWar || Casting.LastSpell != Spells.Adloquium || Casting.LastSpell != Spells.Succor)
            //    if (await Spells.Ruin2.Cast(Core.Me.CurrentTarget))
            //        return true;
        }
    }
}
