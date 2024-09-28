using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	[AutoloadEquip(EquipType.Shoes)]
	public class TerraBooster : ModItem//HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Terra Booster");
			//Tooltip.SetDefault("Allows the user to run insanely fast and extra mobility on ice\n" +
			/*"Allows somersaulting\n" +
			"Damage enemies while running or somersaulting\n" +
			"Damage scales off of enemy's contact damage\n" +
			"Allows the user to jump up to 10 times in a row\n" +
			"Jumps recharge mid-air\n" +
			"Holding left/right while jumping midair gives a boost\n" + 
			"Provides the ability to walk on water and lava\n" + 
			"Grants immunity to fire blocks and 7 seconds lava immunity\n" +
			"Increases jump height and prevents fall damage");*/

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.noMelee = true;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 36;
			Item.height = 32;
			Item.maxStack = 1;
			Item.value = 250000;
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			/*
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			*/
		}

		public override void AddRecipes()
		{
			// TODO: include soaring insignia?
			CreateRecipe(1)
				.AddIngredient<ScrewSpaceBooster>(1)
				.AddIngredient(ItemID.TerrasparkBoots, 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			CreateRecipe(1)
				.AddIngredient<ScrewSpaceBooster>(1)
				.AddIngredient(ItemID.FrostsparkBoots, 1)
				.AddIngredient(ItemID.LavaWaders, 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			player.accRunSpeed = 6.75f;
			player.moveSpeed += 0.2f;
			player.iceSkate = true;
			player.waterWalk = true;
			player.fireWalk = true;
			player.lavaMax += 420;
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item), mp.speedBoostDmg);
			mp.spaceJump = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item), mp.screwAttackDmg);
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
	[AutoloadEquip(EquipType.Shoes)]
	public class TerraBoosterV2 : ModItem
	{
		public override string Texture => $"{Mod.Name}/Content/Items/Accessories/TerraBoosterV2";
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.noMelee = true;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 36;
			Item.height = 32;
			Item.maxStack = 1;
			Item.value = 250000;
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			/*
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			*/
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			player.accRunSpeed = 6.75f;
			player.moveSpeed += 0.2f;
			player.iceSkate = true;
			player.waterWalk = true;
			player.fireWalk = true;
			player.lavaMax += 420;
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item), mp.speedBoostDmg);
			mp.spaceJump = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item), mp.screwAttackDmg);
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
			player.wingTime = player.wingTimeMax;
			mp.insigniaActive = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<TerraBooster>(1)
				.AddIngredient(ItemID.EmpressFlightBooster, 1)
				.AddIngredient(ItemID.LunarBar, 2)
				.Register();
		}
	}
}
