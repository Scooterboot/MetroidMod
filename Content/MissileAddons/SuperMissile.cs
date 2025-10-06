using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Projectiles;
using MetroidMod.Default;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons
{
	internal class SuperMissile : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;

		
		public override Color PrimaryColor => MetroidMod.iceColor;

		public override Color SecondaryColor => MetroidMod.iceSecondaryColor;
		public override int ShotDust => DustID.IceTorch;

		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Primary;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = 30000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 8)
				.AddIngredient(ItemID.SoulofNight, 5)
				//.AddIngredient<Tiles.MissileExpansion>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
