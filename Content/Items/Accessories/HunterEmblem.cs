using MetroidMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	public class HunterEmblem : ModItem
	{
		//All the important numbers changes need to be up here so the dynamic localization thing can access them.
		//They're written as the percent changes so it's easier for the thing to read them
		//On the plus side it'll make changing stats easier!   -Z
		public static float huntDamage = 15f; //percent increase to hunter damage

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(huntDamage);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hunter Emblem");
			// Tooltip.SetDefault("15% increased hunter damage");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 100000;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (incomingItem.type == ModContent.ItemType<SupercooledEmblem>() || incomingItem.type == ModContent.ItemType<FrozenCore>() || incomingItem.type == ModContent.ItemType<EnhancedCombatUnit>())
			{
				return false;
			}
			return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100; //formula to convert huntDamage to a percent value
		}

		public override void AddRecipes()
		{
			Recipe.Create(ItemID.AvengerEmblem)
				.AddIngredient(Type)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddIngredient(ItemID.SoulofSight, 5)
				.AddIngredient(ItemID.SoulofFright, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(item.type);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
			recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(ItemID.AvengerEmblem);
			recipe.AddRecipe();*/
		}
	}
}
