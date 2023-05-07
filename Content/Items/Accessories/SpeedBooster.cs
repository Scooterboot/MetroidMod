using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class SpeedBooster : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Speed Booster");
			// Tooltip.SetDefault("[c/ff0000:Unobtainable.] Please use the Suit Addon system.");
			/*"Allows the user to run insanely fast\n" + 
			"Damages enemies while running\n" +
			"Damage scales off of enemy's contact damage\n" +
			"While active, press DOWN to charge a Shine Spark\n" +
			"Then press JUMP to activate the charge");*/

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.SpeedBoosterTile>();
		}
		/*
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.SerrisCoreX>()
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.JungleSpores, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
		*/
		public override bool CanRightClick() => true;
		public override void RightClick(Player player)
		{
			var entitySource = player.GetSource_OpenItem(Type);

			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.SpeedBooster>().ItemType);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item),mp.speedBoostDmg);
		}
	}
}
