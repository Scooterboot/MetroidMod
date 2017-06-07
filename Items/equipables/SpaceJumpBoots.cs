using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Items.equipables
{
	public class SpaceJumpBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Jump Boots");
			Tooltip.SetDefault("Allows the wearer to double jump\n" + 
			"Allows somersaulting");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 40000;
			item.rare = 3;
			item.accessory = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SpaceJumpBootsTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(ItemID.Emerald, 2);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.AddSpaceJump(player);
		}
		
	}
}