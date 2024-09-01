using System;
using MetroidMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MetroidMod.Content.Items.Accessories
{
	// legacy name because old suit addon system
	[LegacyName("SpeedBoosterAddon")]
	public class SpeedBooster : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Speed Booster");
			//Tooltip.SetDefault("Allows the user to run insanely fast\n" + 
			/*"Damages enemies while running\n" +
			"Damage scales off of enemy's contact damage\n" +
			"While active, press DOWN to charge a Shine Spark\n" +
			"Then press JUMP to activate the charge");*/

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.noMelee = true;
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
			
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.SpeedBoosterTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.SerrisCoreX>()
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.JungleSpores, 5)
				.AddIngredient(ItemID.Magiluminescence, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item), mp.speedBoostDmg);
		}
	}
}
