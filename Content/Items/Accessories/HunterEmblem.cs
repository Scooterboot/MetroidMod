using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	public class HunterEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunter Emblem");
			Tooltip.SetDefault("15% increased hunter damage");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 100000;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Common.Players.HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.15f;
		}
		
		public override void AddRecipes()
		{
			Mod.CreateRecipe(ItemID.AvengerEmblem)
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
