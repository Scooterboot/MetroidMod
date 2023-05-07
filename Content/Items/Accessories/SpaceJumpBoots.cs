using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class SpaceJumpBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Space Jump Boots");
			// Tooltip.SetDefault("[c/ff0000:Unobtainable.] Please use the Suit Addon system.");
			/*"Allows the wearer to double jump\n" + 
			"Allows somersaulting\n" +
			"Increases jump height");*/

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = mod.TileType("SpaceJumpBootsTile");
		}

		/*public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.CloudinaBottle, 1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.BlizzardinaBottle, 1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.SandstorminaBottle, 1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.TsunamiInABottle, 1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();
		}*/
		public override bool CanRightClick() => true;
		public override void RightClick(Player player)
		{
			var entitySource = player.GetSource_OpenItem(Type);

			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.SpaceJumpBoots>().ItemType);
			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.HiJumpBoots>().ItemType);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			mp.hiJumpBoost = true;
		}
	}
}
