using Magitek.Models.Roles;
using PropertyChanged;
using System.ComponentModel;
using System.Configuration;

namespace Magitek.Models.Gunbreaker
{
    [AddINotifyPropertyChangedInterface]
    public class GunbreakerSettings : TankSettings, IRoutineSettings
    {
        public GunbreakerSettings() : base(CharacterSettingsDirectory + "/Magitek/Gunbreaker/GunbreakerSettings.json") { }

        public static GunbreakerSettings Instance { get; set; } = new GunbreakerSettings();

        [Setting]
        [DefaultValue(true)]
        public bool UseAmmoCombo { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseRoyalGuard { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseSuperbolide { get; set; }

        [Setting]
        [DefaultValue(5)]
        public int SuperbolideHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseCamouflage { get; set; }

        [Setting]
        [DefaultValue(10)]
        public int CamouflageHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseNebula { get; set; }

        [Setting]
        [DefaultValue(30)]
        public int NebulaHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseHeartofLight { get; set; }

        [Setting]
        [DefaultValue(10)]
        public int HeartofLightHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseHeartofCorundum { get; set; }

        [Setting]
        [DefaultValue(15)]
        public int HeartofCorundumHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAurora { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAuroraHealer { get; set; }

        [Setting]
        [DefaultValue(30)]
        public int UseAuroraHealerHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAuroraDps { get; set; }

        [Setting]
        [DefaultValue(30)]
        public int UseAuroraDpsHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAuroraSelf { get; set; }

        [Setting]
        [DefaultValue(30)]
        public int UseAuroraSelfHealthPercent { get; set; }

        [Setting]
        [DefaultValue(15)]
        public int AuroraAsDefensiveHealthPercent { get; set; }

        [Setting]
        [DefaultValue(50)]
        public int MinMpAurora { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAoe { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int DemonSliceSlaughterEnemies { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseFatedCircle { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseBowShock { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int FatedCircleEnemies { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseNoMercy { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseRoughDivide { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseBloodfest { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool PullWithLightningShot { get; set; }

        [Setting]
        [DefaultValue(0)]
        public int LightningShotMinDistance { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool LightningShotToPullAggro { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool SaveBlastingZone { get; set; }

        [Setting]
        [DefaultValue(6000)]
        public int SaveBlastingZoneMseconds { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseDoubleDown { get; set; }

        [Setting]
        [DefaultValue(1)]
        public int DoubleDownEnemies { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseBurstStrike { get; set; }
    }
}