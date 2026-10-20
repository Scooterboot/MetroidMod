using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace MetroidMod.Content.Items.Weapons.ArmCannon
{
	/// <summary>
	/// </summary>
	public interface IArmCannon
	{
		Item[] BeamAddons { get; }
		Item[] ChargeBeamHotswap { get; }
		Item[] MissileAddons { get; }
		Item[] MissileChargeHotswap { get; }

		ArmCannonUIConfig UIConfiguration { get; }

		/// <summary>
		/// Configuration to be thrown into the Arm Cannon UI.
		/// </summary>
		class ArmCannonUIConfig
		{
			public Rectangle[] BeamLocations;
			public Rectangle[] MissileLocations;

			public Asset<Texture2D> WeaponTexture;
			public Asset<Texture2D> Lines;
		}
	}
}
