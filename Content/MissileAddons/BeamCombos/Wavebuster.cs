using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class Wavebuster : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => DustID.YellowTorch;
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 10)
				.AddIngredient(ItemID.SoulofNight, 1)
				.AddIngredient(ItemID.Amethyst, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
