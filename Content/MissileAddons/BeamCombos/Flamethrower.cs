using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class Flamethrower : ModMissileAddon
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
			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.Ichor, 10)
				.AddIngredient(ItemID.Ruby, 1)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.CursedFlame, 10)
				.AddIngredient(ItemID.Ruby, 1)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
