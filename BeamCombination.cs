using Terraria.Audio;

namespace MetroidModPorted
{
	public class BeamCombination
	{
		internal int beam1;
		internal int beam2;
		internal int beam3;
		internal int beam4;
		internal int beam5;
		internal int projectileType;
		internal string projectileTexture;
		internal LegacySoundStyle shotSound;
		internal LegacySoundStyle chargeUpSound;
		internal LegacySoundStyle chargeShotSound;

		/// <summary> Set Projectile id for combinations of beams. </summary>
		public BeamCombination SetProjectileType(int projectileType)
		{
			this.projectileType = projectileType;
			return this;
		}

		/// <summary> Set shoot sound for combinations of beams. </summary>
		public BeamCombination SetShotSound(LegacySoundStyle shotSound)
		{
			this.shotSound = shotSound;
			return this;
		}

		/// <summary> Set shoot sound for combinations of beams. </summary>
		public BeamCombination SetChargingSound(LegacySoundStyle chargeUpSound)
		{
			this.chargeUpSound = chargeUpSound;
			return this;
		}

		/// <summary> Set shoot sound for combinations of beams. </summary>
		public BeamCombination SetChargeShotSound(LegacySoundStyle chargeShotSound)
		{
			this.chargeShotSound = chargeShotSound;
			return this;
		}

		internal BeamCombination(int beam1 = -1, int beam2 = -1, int beam3 = -1, int beam4 = -1, int beam5 = -1)
		{
			if (beam1 > -1) { this.beam1 = beam1; }
			if (beam2 > -1) { this.beam2 = beam2; }
			if (beam3 > -1) { this.beam3 = beam3; }
			if (beam4 > -1) { this.beam4 = beam4; }
			if (beam5 > -1) { this.beam5 = beam5; }
		}

		internal bool Is(int beam1, int beam2, int beam3, int beam4, int beam5, out BeamCombination beamCombination)
		{
			beamCombination = this;
			return
				!(
					(this.beam1 != beam1 && this.beam1 != beam2 && this.beam1 != beam3 && this.beam1 != beam4 && this.beam1 != beam5) ||
					(this.beam2 != beam1 && this.beam2 != beam2 && this.beam2 != beam3 && this.beam2 != beam4 && this.beam2 != beam5) ||
					(this.beam3 != beam1 && this.beam3 != beam2 && this.beam3 != beam3 && this.beam3 != beam4 && this.beam3 != beam5) ||
					(this.beam4 != beam1 && this.beam4 != beam2 && this.beam4 != beam3 && this.beam4 != beam4 && this.beam4 != beam5) ||
					(this.beam5 != beam1 && this.beam5 != beam2 && this.beam5 != beam3 && this.beam5 != beam4 && this.beam5 != beam5)
				)
				//(this.beam1 == beam1 && this.beam2 == beam2 && this.beam3 == beam3 && this.beam4 == beam4 && this.beam5 == beam5) ||
				//(this.beam1 == beam2 && this.beam2 == beam1 && this.beam3 == beam3 && this.beam4 == beam4 && this.beam5 == beam5)
			;
		}
	}
}
