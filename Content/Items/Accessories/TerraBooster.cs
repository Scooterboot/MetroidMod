using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;

namespace MetroidMod.Content.Items.Accessories
{
	public class TerraBooster : ModItem//HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Booster");
			Tooltip.SetDefault("[c/ff0000:Unobtainable.] Please use the Suit Addon system.");
			/*"Allows the user to run insanely fast and extra mobility on ice\n" +
			"Allows somersaulting\n" +
			"Damage enemies while running or somersaulting\n" +
			"Damage scales off of enemy's contact damage\n" +
			"Allows the user to jump up to 10 times in a row\n" +
			"Jumps recharge mid-air\n" +
			"Holding left/right while jumping midair gives a boost\n" + 
			"Provides the ability to walk on water and lava\n" + 
			"Grants immunity to fire blocks and 7 seconds lava immunity\n" +
			"Increases jump height and prevents fall damage");*/

			SacrificeTotal = 1;
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
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		/*
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ScrewSpaceBooster>(1)
				.AddIngredient(ItemID.TerrasparkBoots, 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
		*/
		public override bool CanRightClick() => true;
		public override void RightClick(Player player)
		{
			var entitySource = player.GetSource_OpenItem(Type);

			player.QuickSpawnItem(entitySource, ItemID.TerrasparkBoots);
			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.ScrewAttack>().ItemType);
			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.SpaceJump>().ItemType);
			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.SpeedBooster>().ItemType);
			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.HiJumpBoots>().ItemType);
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
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item),mp.speedBoostDmg);
			mp.spaceJump = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item),mp.screwAttackDmg);
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
}
