using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons
{
	internal class IceSuper : ModMissileAddon
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
				.AddMissileAddon<SuperMissile>(1)
				.AddMissileAddon<IceMissile>(1)
				.AddIngredient(ItemID.Ectoplasm, 8)
				.AddIngredient(ItemID.BeetleHusk, 3)
				.AddIngredient<FrozenCore>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
