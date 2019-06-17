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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Jump");
			Tooltip.SetDefault("'Somersault continuously in the air!'\n" + 
			"Allows somersaulting\n" + 
			"Allows the user to jump up to 10 times in a row\n" + 
			"Jumps recharge mid-air");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 40000;
			item.rare = 7;
			item.accessory = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SpaceJumpTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpaceJumpBoots");
			recipe.AddIngredient(ItemID.BundleofBalloons);
			recipe.AddIngredient(ItemID.RocketBoots);
			recipe.AddIngredient(ItemID.SoulofFlight, 10);
            		recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.spaceJump = true;
		}
		
		public override bool CanEquipAccessory(Player player, int slot)
		{
			for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
			{
				if(k != slot && (player.armor[k].type == mod.ItemType("SpaceBooster") || player.armor[k].type == mod.ItemType("ScrewSpaceBooster") || player.armor[k].type == mod.ItemType("TerraBooster")))
				{
				    return false;
				}
			}
			return true;
		}
	}
}