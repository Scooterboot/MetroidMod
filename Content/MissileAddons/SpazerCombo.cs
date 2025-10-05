using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.MissileAddons.BeamCombos
{
	public class SpazerCombo : ModMissileAddon
	{
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => DustID.YellowTorch;

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Bone, 10)
				.AddTile(TileID.Anvils)
				//.AddDecraftCondition(Condition.DownedSkeletron)
				.Register();
		}
	}
}
