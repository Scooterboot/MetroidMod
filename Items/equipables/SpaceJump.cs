using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Items.equipables;

namespace MetroidMod.Items.equipables
{
	public class SpaceJump : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Space Jump";
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.toolTip = "'Somersault continuously in the air!'\n" + 
			"Allows somersaulting\n" + 
			"Allows the user to jump up to 10 times in a row\n" + 
			"Jumps recharge mid-air";
			item.value = 40000;
			item.rare = 7;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpaceJumpBoots");
			recipe.AddIngredient("Meteorite Bar", 3);
			recipe.AddIngredient("Bundle of Balloons");
			recipe.AddIngredient("Rocket Boots");
			recipe.AddIngredient("Soul of Flight", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.AddSpaceJumping(player);
		}
		
		public override bool CanEquipAccessory(Player player, int slot)
		{
			 for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
            {
                if (player.armor[k].type == mod.ItemType("SpaceBooster"))
                {
                    return false;
                }
            }
return true;
		
		}
	}
}