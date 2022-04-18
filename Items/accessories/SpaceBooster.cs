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
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.accessories
{
	public class SpaceBooster : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Booster");
			Tooltip.SetDefault("Allows the user to run insanely fast\n" + 
			"Damage enemies while running\n" + 
			"Damage scales off of enemy's contact damage\n" +
			"Allows the user to jump up to 10 times in a row\n" + 
			"Jumps recharge mid-air\n" + 
			"Allows somersaulting & wall jumping\n" +
			"Increases jump height and prevents fall damage");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 75;
			item.noMelee = true;
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
			item.createTile = mod.TileType("SpaceBoosterTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpaceJump");
			recipe.AddIngredient(null, "SpeedBooster");
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(item),mp.speedBoostDmg);
			mp.spaceJump = true;
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
}