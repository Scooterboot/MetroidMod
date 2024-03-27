using Terraria.Audio;

namespace MetroidMod
{
	public static partial class Sounds
	{
		public static class Suit
		{
			public static readonly SoundStyle WallJump = new($"{nameof(MetroidMod)}/Assets/Sounds/WallJump")
			{
				Volume = 0.25f,
				PitchVariance = 0.15f,
				MaxInstances = 4
			};

			public static readonly SoundStyle GripHang = new($"{nameof(MetroidMod)}/Assets/Sounds/GripHang")
			{
				Volume = 0.25f,
				PitchVariance = 0.15f,
				MaxInstances = 4
			};

			public static readonly SoundStyle GripClimb = new($"{nameof(MetroidMod)}/Assets/Sounds/GripClimb")
			{
				Volume = 0.25f,
				PitchVariance = 0.15f,
				MaxInstances = 4
			};

			public static readonly SoundStyle SenseMove = new($"{nameof(MetroidMod)}/Assets/Sounds/SenseMoveSound")
			{

			};
			public static readonly SoundStyle Sting = new($"{nameof(MetroidMod)}/Assets/Sounds/Sting")
			{

			};
			public static readonly SoundStyle SamusDeath = new($"{nameof(MetroidMod)}/Assets/Sounds/SamusDeath")
			{

			};
			public static readonly SoundStyle ConcentrationLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/ConcentrationLoop")
			{

			};

			public static readonly SoundStyle MissilesReplenished = new($"{nameof(MetroidMod)}/Assets/Sounds/MissilesReplenished")
			{

			};

			public static readonly SoundStyle EnergyPickup = new($"{nameof(MetroidMod)}/Assets/Sounds/EnergyPickupSound")
			{

			};

			public static readonly SoundStyle MissilePickup = new($"{nameof(MetroidMod)}/Assets/Sounds/MissilePickupSound") 
			{
					
			};

			public static readonly SoundStyle UAPickup = new($"{nameof(MetroidMod)}/Assets/Sounds/UAPickup")
			{

			};

			public static readonly SoundStyle EnergyHit = new($"{nameof(MetroidMod)}/Assets/Sounds/EnergyHit_", 0, 2)
			{
				PitchVariance = 0.5f
			};

			public static readonly SoundStyle EnergyLow = new($"{nameof(MetroidMod)}/Assets/Sounds/EnergyLow")
			{

			};

			public static readonly SoundStyle MorphIn = new($"{nameof(MetroidMod)}/Assets/Sounds/MorphIn")
			{

			};

			public static readonly SoundStyle MorphOut = new($"{nameof(MetroidMod)}/Assets/Sounds/MorphOut")
			{

			};

			public static readonly SoundStyle LayBomb = new($"{nameof(MetroidMod)}/Assets/Sounds/LayBomb")
			{

			};

			public static readonly SoundStyle LayPowerBomb = new($"{nameof(MetroidMod)}/Assets/Sounds/LayPowerBomb")
			{

			};

			public static readonly SoundStyle BombExplode = new($"{nameof(MetroidMod)}/Assets/Sounds/BombExplode")
			{

			};

			public static readonly SoundStyle BoostBallStartup = new($"{nameof(MetroidMod)}/Assets/Sounds/BoostBallStartup")
			{

			};

			public static readonly SoundStyle BoostBallLoop = new($"{nameof(MetroidMod)}/Assets/Sounds/BoostBallLoop")
			{

			};

			public static readonly SoundStyle BoostBallSound = new($"{nameof(MetroidMod)}/Assets/Sounds/BoostBallSound")
			{

			};

			public static readonly SoundStyle SpiderActivate = new($"{nameof(MetroidMod)}/Assets/Sounds/SpiderActivate")
			{

			};
			public static readonly SoundStyle SpawnIn = new($"{nameof(MetroidMod)}/Assets/Sounds/SpawnIn")
			{

			};

			public static class Visors
			{
				public static readonly SoundStyle ScanVisorBackgroundNoise = new($"{nameof(MetroidMod)}/Assets/Sounds/ScanVisorBackgroundNoise", SoundType.Ambient)
				{
					IsLooped = true
				};

				public static readonly SoundStyle ScanVisorScanning = new($"{nameof(MetroidMod)}/Assets/Sounds/ScanVisorScanning")
				{
					IsLooped = true
				};

				public static readonly SoundStyle DarkVisorBackgroundNoise = new($"{nameof(MetroidMod)}/Assets/Sounds/DarkVisorBackgroundNoise", SoundType.Ambient)
				{
					IsLooped = true
				};
			}
		}
	}
}
