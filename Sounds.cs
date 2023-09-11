using Terraria.Audio;

namespace MetroidMod
{
	public static partial class Sounds
	{
		public static class Tiles
		{
			public static readonly SoundStyle HatchOpen = new($"{nameof(MetroidMod)}/Assets/Sounds/HatchOpenSound")
			{

			};

			public static readonly SoundStyle HatchClose = new($"{nameof(MetroidMod)}/Assets/Sounds/HatchCloseSound")
			{

			};
		}
		public static class Items
		{
			public static readonly SoundStyle SerrisSummon = new($"{nameof(MetroidMod)}/Assets/Sounds/SerrisSummon")
			{

			};

			public static class Tools
			{
				public static readonly SoundStyle GrappleBeamSound = new($"{nameof(MetroidMod)}/Assets/Sounds/GrappleBeamSound")
				{
					MaxInstances = 12
				};

				public static readonly SoundStyle GrappleLatch = new($"{nameof(MetroidMod)}/Assets/Sounds/GrappleLatch")
				{

				};
			}

			public static class Weapons
			{
				public static readonly SoundStyle PowerBeamSound = new($"{nameof(MetroidMod)}/Assets/Sounds/PowerBeamSound")
				{
					MaxInstances = 12
				};

				public static readonly SoundStyle NebulaComboSoundLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/NebulaComboSoundLoop")
				{
					IsLooped = true
				};

				public static readonly SoundStyle NovaLaserLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/NovaLaserLoop")
				{
					IsLooped = true
				};

				public static readonly SoundStyle PhazonBeamSound = new($"{nameof(MetroidMod)}/Assets/Sounds/PhazonBeamSound")
				{
					MaxInstances = 12,
					IsLooped = true
				};

				public static readonly SoundStyle SolarComboSoundStart = new($"{nameof(MetroidMod)}/Assets/Sounds/SolarComboSoundStart")
				{
					
				};

				public static readonly SoundStyle SolarComboSoundLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/SolarComboSoundLoop")
				{
					IsLooped = true
				};

				public static readonly SoundStyle VortexComboSoundLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/VortexComboSoundLoop")
				{
					IsLooped = true
				};

				public static readonly SoundStyle MissileSound = new($"{nameof(MetroidMod)}/Assets/Sounds/MissileSound")
				{

				};

				public static readonly SoundStyle MissileShoot = new($"{nameof(MetroidMod)}/Assets/Sounds/MissileShoot")
				{

				};

				public static readonly SoundStyle MissileExplode = new($"{nameof(MetroidMod)}/Assets/Sounds/MissileExplode")
				{

				};

				public static readonly SoundStyle SuperMissileShoot = new($"{nameof(MetroidMod)}/Assets/Sounds/SuperMissileShoot")
				{

				};

				public static readonly SoundStyle SuperMissileExplode = new($"{nameof(MetroidMod)}/Assets/Sounds/SuperMissileExplode")
				{

				};

				public static readonly SoundStyle IceMissileShoot = new($"{nameof(MetroidMod)}/Assets/Sounds/IceMissileShoot")
				{

				};

				public static readonly SoundStyle IceMissileExplode = new($"{nameof(MetroidMod)}/Assets/Sounds/IceMissileExplode")
				{

				};

				public static readonly SoundStyle ChargeComboActivate = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeComboActivate")
				{

				};

				public static readonly SoundStyle ChargeStartup_Seeker = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeStartup_Seeker")
				{

				};

				public static readonly SoundStyle SeekerLockSound = new($"{nameof(MetroidMod)}/Assets/Sounds/SeekerLockSound")
				{

				};

				public static readonly SoundStyle SeekerMissileSound = new($"{nameof(MetroidMod)}/Assets/Sounds/SeekerMissileSound")
				{

				};

				public static readonly SoundStyle FlamethrowerStart = new($"{nameof(MetroidMod)}/Assets/Sounds/FlamethrowerStart")
				{

				};

				public static readonly SoundStyle FlamethrowerLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/FlamethrowerLoop")
				{

				};

				public static readonly SoundStyle IceSpreaderImpactSound = new($"{nameof(MetroidMod)}/Assets/Sounds/IceSpreaderImpactSound")
				{

				};

				public static readonly SoundStyle StardustAfterImpactSound = new($"{nameof(MetroidMod)}/Assets/Sounds/StardustAfterImpactSound")
				{

				};

				public static readonly SoundStyle WavebusterStart = new($"{nameof(MetroidMod)}/Assets/Sounds/WavebusterStart")
				{

				};

				public static readonly SoundStyle WavebusterLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/WavebusterLoop")
				{

				};

				public static readonly SoundStyle ChargeMax = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeMax")
				{

				};

				public static readonly SoundStyle ScrewAttack = new($"{nameof(MetroidMod)}/Assets/Sounds/ScrewAttackSound")
				{

				};

				public static readonly SoundStyle ScrewAttackSpeed = new($"{nameof(MetroidMod)}/Assets/Sounds/ScrewAttackSpeedSound")
				{

				};

				public static readonly SoundStyle SpeedBoosterStartup = new($"{nameof(MetroidMod)}/Assets/Sounds/SpeedBoosterStartup")
				{

				};

				public static readonly SoundStyle SpeedBoosterLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/SpeedBoosterLoop")
				{

				};

				public static readonly SoundStyle ShineSpark = new($"{nameof(MetroidMod)}/Assets/Sounds/ShineSpark")
				{

				};

				public static readonly SoundStyle Paralyzer = new($"{nameof(MetroidMod)}/Assets/Sounds/Paralyzer")
				{

				};

                public static readonly SoundStyle HomingMissileShoot = new($"{nameof(MetroidMod)}/Assets/Sounds/HomingMissileShoot")
                {

                };

                public static readonly SoundStyle ChargeStartup_HomingMissile = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeStartup_HomingMissile")
                {

                };

                public static readonly SoundStyle MissileShootHunters = new($"{nameof(MetroidMod)}/Assets/Sounds/MissileShootHunters")
                {

                };

                public static readonly SoundStyle MissileExplodeHunters = new($"{nameof(MetroidMod)}/Assets/Sounds/MissileExplodeHunters")
                {

                };

                public static readonly SoundStyle VoltDriverShot = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverSound")
				{

				};

				public static readonly SoundStyle VoltDriverImpactSound = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverImpactSound")
				{

				};
				public static readonly SoundStyle VoltDriverDaze = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverDaze")
				{

				};

				public static readonly SoundStyle VoltDriverChargeSound = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverChargeSound")
				{

				};
				public static readonly SoundStyle VoltDriverCharge = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverCharge")
				{

				};

				public static readonly SoundStyle VoltDriverChargeImpactSound = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverChargeImpactSound")
				{

				};

				public static readonly SoundStyle JudicatorSound = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorSound")
				{

				};

				public static readonly SoundStyle JudicatorImpactSound = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorImpactSound")
				{

				};
				public static readonly SoundStyle JudicatorFreeze = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorFreeze")
				{

				};

				public static readonly SoundStyle JudicatorChargeSound = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorChargeSound")
				{

				};

				public static readonly SoundStyle JudicatorAffinityChargeSound = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorAffinityChargeSound")
				{

				};

				public static readonly SoundStyle ChargeStartup_JudicatorAffinity = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeStartup_JudicatorAffinity")
				{

				};
				
				public static readonly SoundStyle JudicatorAffinityChargeShot = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorAffinityChargeShot")
				{

				};

				public static readonly SoundStyle BattleHammerSound = new($"{nameof(MetroidMod)}/Assets/Sounds/BattleHammerSound")
				{

				};

				public static readonly SoundStyle BattleHammerAffinitySound = new($"{nameof(MetroidMod)}/Assets/Sounds/BattleHammerAffinitySound")
				{

				};

				public static readonly SoundStyle BattleHammerImpactSound = new($"{nameof(MetroidMod)}/Assets/Sounds/BattleHammerImpactSound")
				{

				};

				public static readonly SoundStyle MagMaulSound = new($"{nameof(MetroidMod)}/Assets/Sounds/MagMaulSound")
				{

				};

				public static readonly SoundStyle MagMaulExplode = new($"{nameof(MetroidMod)}/Assets/Sounds/MagMaulExplode")
				{

				};

				public static readonly SoundStyle ChargeStartup_MagMaul = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeStartup_MagMaul")
				{

				};

				public static readonly SoundStyle ImperialistSound = new($"{nameof(MetroidMod)}/Assets/Sounds/ImperialistSound")
				{

				};

				public static readonly SoundStyle ShockCoilSound = new($"{nameof(MetroidMod)}/Assets/Sounds/ShockCoilSound")
				{

				};

				public static readonly SoundStyle ShockCoilAffinity1 = new($"{nameof(MetroidMod)}/Assets/Sounds/ShockCoilAffinity1")
				{

				};

				public static readonly SoundStyle ShockCoilAffinity2 = new($"{nameof(MetroidMod)}/Assets/Sounds/ShockCoilAffinity2")
				{

				};

                public static readonly SoundStyle ShockCoilStartupSound = new($"{nameof(MetroidMod)}/Assets/Sounds/ShockCoilStartupSound")
                {

                };

                public static readonly SoundStyle OmegaCannonShot = new($"{nameof(MetroidMod)}/Assets/Sounds/OmegaCannonShot")
                {

                };

				public static readonly SoundStyle ShockCoilLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/ShockCoilLoad")
				{

				};

				public static readonly SoundStyle ShockCoilReload = new($"{nameof(MetroidMod)}/Assets/Sounds/ShockCoilReload")
				{

				};

				public static readonly SoundStyle BattleHammerLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/BattleHammerLoadFake")
				{

				};

				public static readonly SoundStyle ChargeBeamLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/ChargeBeamLoad")
				{

				};

				public static readonly SoundStyle ImperialistLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/ImperialistLoad")
				{

				};

				public static readonly SoundStyle JudicatorLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/JudicatorLoad")
				{

				};

				public static readonly SoundStyle MagMaulLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/MagMaulLoad")
				{

				};

				public static readonly SoundStyle OmegaCannonLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/OmegaCannonLoad")
				{

				};

				public static readonly SoundStyle VoltDriverLoad = new($"{nameof(MetroidMod)}/Assets/Sounds/VoltDriverLoad")
				{

				};

				public static readonly SoundStyle BeamSelect = new($"{nameof(MetroidMod)}/Assets/Sounds/BeamSelect")
				{

				};

				public static readonly SoundStyle BeamSelectFail = new($"{nameof(MetroidMod)}/Assets/Sounds/BeamSelectFail")
				{

				};

				public static readonly SoundStyle BeamAquired = new($"{nameof(MetroidMod)}/Assets/Sounds/BeamAquired")
				{

				};
			}
		}
	}
}
