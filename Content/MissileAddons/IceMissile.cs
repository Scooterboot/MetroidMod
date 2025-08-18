using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons
	{
	internal class IceMissile : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;

		public override Color PrimaryColor => MetroidMod.iceColor;

		public override Color SecondaryColor => MetroidMod.iceSecondaryColor;
		public override int ShotDust => DustID.IceTorch;

		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Primary;

			#region Visual Priority
			ShapePriority = 5;
			#endregion
			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
		}
		public override void SetItemDefaults(Item item)
		{
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 1, 98, 7); //markiplier.jpeg
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(3)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 8)
				.AddIngredient(ItemID.Amethyst, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
