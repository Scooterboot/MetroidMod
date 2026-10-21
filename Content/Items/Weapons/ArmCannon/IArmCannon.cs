using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

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
			public Vector2[] BeamLocations = [
				new(35, 18),
				new(34, 188),
				new(202, 188),
				new(34, 118),
				new(202, 118),
				new(90, 276)
			];
			public Vector2[] MissileLocations = [
				new(118, 28),
				new(201, 18),
				new(146, 276)
			];

			public Asset<Texture2D> Wireframe = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Frame");
			public Asset<Texture2D> Lines = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Lines");
		}
	}
}
