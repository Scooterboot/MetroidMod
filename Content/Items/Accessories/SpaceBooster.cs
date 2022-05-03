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
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.DamageClasses;

namespace MetroidModPorted.Content.Items.Accessories
{
	public class SpaceBooster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Booster");
			Tooltip.SetDefault("Allows the user to run insanely fast\n" + 
			"Damage enemies while running\n" + 
			"Damage scales off of enemy's contact damage\n" +
			"Allows the user to jump up to 10 times in a row\n" + 
			"Jumps recharge mid-air\n" + 
			"Allows somersaulting\n" +
			"Increases jump height and prevents fall damage");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.noMelee = true;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = 7;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			//Item.consumable = true;
			//Item.createTile = mod.TileType("SpaceBoosterTile");
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<SpaceJump>(1)
				.AddIngredient<SpeedBooster>(1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpaceJump");
			recipe.AddIngredient(null, "SpeedBooster");
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item),mp.speedBoostDmg);
			mp.spaceJump = true;
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
}
