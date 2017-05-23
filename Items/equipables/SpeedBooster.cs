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

namespace MetroidMod.Items.equipables
{
	public class SpeedBooster : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Speed Booster";
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.toolTip = "Allows the user to run insanely fast\n" + 
			"Damages enemies while running";
			item.value = 40000;
			item.rare = 5;
			item.accessory = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SpeedBoosterTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Hermes Boots");
			recipe.AddIngredient("Spectre Boots");
			recipe.AddIngredient(null, "SerrisCoreX");
			recipe.AddIngredient("Emerald", 5);
			recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.speedBooster = true;
			mp.AddSpeedBoost(player);
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
