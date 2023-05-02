using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;

namespace MetroidMod.Content.Items.Accessories
{
	public class ScrewSpaceBooster : ModItem//HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Boosted Screw Attack");
			Tooltip.SetDefault("Allows the user to run insanely fast\n" +
			"Allows somersaulting\n" +
			"Damage enemies while running or somersaulting\n" +
			"Damage scales off of enemy's contact damage\n" +
			"Allows the user to jump up to 10 times in a row\n" +
			"Jumps recharge mid-air\n" +
			"Holding left/right while jumping midair gives a boost\n" +
			"Increases jump height and prevents fall damage");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.noMelee = true;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 34;
			Item.height = 38;
			Item.maxStack = 1;
			Item.value = 80000;
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ScrewAttack>(1)
				.AddIngredient<SpaceBooster>(1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item),mp.speedBoostDmg);
			mp.spaceJump = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item),mp.screwAttackDmg);
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
}
