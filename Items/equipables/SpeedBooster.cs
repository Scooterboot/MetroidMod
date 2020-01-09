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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
			Tooltip.SetDefault("Allows the user to run insanely fast\n" + 
			"Damages enemies while running");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
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
			recipe.AddIngredient(ItemID.HermesBoots);
			recipe.AddIngredient(ItemID.SpectreBoots);
			recipe.AddIngredient(null, "SerrisCoreX");
			recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
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