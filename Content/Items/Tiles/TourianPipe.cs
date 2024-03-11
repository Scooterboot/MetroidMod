using Terraria;
﻿using MetroidMod.Content.Items.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class TourianPipe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tourian Pipe");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.TourianPipe>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<ChoziteOre>(1)
				.AddIngredient(ItemID.TitaniumOre, 1) //I'm making tourian pipes hardmode-exclusive because titanium makes the most sense as the alloy material
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddTile(TileID.AdamantiteForge)
				.Register();

			CreateRecipe(20)
				.AddIngredient<ChoziteOre>(1)
				.AddIngredient(ItemID.AdamantiteOre, 1) //it makes more sense with titanium but rng exists
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddTile(TileID.AdamantiteForge)
				.Register();

			CreateRecipe()
				.AddIngredient<TourianWall>(4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
