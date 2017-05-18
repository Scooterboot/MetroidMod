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
		public override void SetDefaults()
		{
			item.name = "Space Jump Boots";
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.toolTip = "Allows the wearer to double jump\n" + 
			"Allows somersaulting";
			item.value = 40000;
			item.rare = 3;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 10);
			recipe.AddIngredient("Fallen Star", 2);
			recipe.AddIngredient("Diamond", 1);
			recipe.AddIngredient("Emerald", 2);
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