using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	/// <summary>
	/// Patches the game to allow a player to move through shimmer with no speed loss
	/// if the ignoreShimmer flag is active.
	/// </summary>
	public class IgnoreShimmerModPlayer : ModPlayer
	{
		/// <summary>
		/// Whether the player should be able to move unhindered through shimmer.
		/// </summary>
		public bool ignoreShimmer;

		public override void Load()
		{
			IL_Player.ShimmerCollision += (il) =>
			{
				float defaultSpeedMultiplier = 0.375f;

				ILCursor c = new(il);

				// Go to the instruction that loads the hardcoded shimmer speed multiplier
				c.GotoNext(i => i.MatchLdcR4(defaultSpeedMultiplier));
				c.Remove();

				// Replace it with our own hardcoded shimmer speed multiplier
				c.EmitLdarg0();
				c.EmitDelegate((Player player) =>
				{
					if (player.TryGetModPlayer(out IgnoreShimmerModPlayer modPlayer))
					{
						if (modPlayer.ignoreShimmer)
						{
							return 1f;
						}
					}

					return defaultSpeedMultiplier;
				});
			};
		}

		public override void ResetEffects()
		{
			ignoreShimmer = false;
		}
	}
}
