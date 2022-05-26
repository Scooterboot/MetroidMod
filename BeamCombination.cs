using System.Collections.Generic;
using Terraria.Audio;

namespace MetroidModPorted
{
	/*
	public class BeamCombination
	{
		internal List<int> beams = new();
		//internal int beam1;
		//internal int beam2;
		//internal int beam3;
		//internal int beam4;
		//internal int beam5;
		internal int projectileType;
		internal string projectileTexture;
		internal SoundStyle shotSound;
		internal SoundStyle chargeUpSound;
		internal SoundStyle chargeShotSound;

		/// <summary> Set Projectile id for combinations of beams. </summary>
		public BeamCombination SetProjectileType(int projectileType)
		{
			this.projectileType = projectileType;
			return this;
		}

		/// <summary> Set shoot sound for combinations of beams. </summary>
		public BeamCombination SetShotSound(SoundStyle shotSound)
		{
			this.shotSound = shotSound;
			return this;
		}

		/// <summary> Set shoot sound for combinations of beams. </summary>
		public BeamCombination SetChargingSound(SoundStyle chargeUpSound)
		{
			this.chargeUpSound = chargeUpSound;
			return this;
		}

		/// <summary> Set shoot sound for combinations of beams. </summary>
		public BeamCombination SetChargeShotSound(SoundStyle chargeShotSound)
		{
			this.chargeShotSound = chargeShotSound;
			return this;
		}

		internal BeamCombination(int beam1 = -1, int beam2 = -1, int beam3 = -1, int beam4 = -1, int beam5 = -1)
		{
			if (beam1 > -1) { beams.Add(beam1); }
			if (beam2 > -1) { beams.Add(beam2); }
			if (beam3 > -1) { beams.Add(beam3); }
			if (beam4 > -1) { beams.Add(beam4); }
			if (beam5 > -1) { beams.Add(beam5); }
		}

		internal bool Is(out BeamCombination beamCombination, int beam1 = -1, int beam2 = -1, int beam3 = -1, int beam4 = -1, int beam5 = -1)
		{
			beamCombination = this;
			bool[] flag1 =
			{
				beam1 != -1,
				beam2 != -1,
				beam3 != -1,
				beam4 != -1,
				beam5 != -1,
			};
			bool[] flag2 =
			{
				beams.Contains(beam1),
				beams.Contains(beam2),
				beams.Contains(beam3),
				beams.Contains(beam4),
				beams.Contains(beam5),
			};
			bool result = true;
			int count = 0;
			for (int i = 0; i < 5; i++)
			{
				if (flag1[i]) { result &= flag2[i]; count += 1; }
			}
			return result && count != 0;
		}
	}
	*/
}
