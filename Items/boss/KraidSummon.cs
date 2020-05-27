using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class KraidSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Variation Matrix");
			Tooltip.SetDefault("Summons Kraid");
		}
		public override void SetDefaults()
		{
			item.maxStack = 20;
			item.consumable = true;
			item.width = 12;
			item.height = 12;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 4;
			item.noMelee = true;
			item.value = 1000;
			item.rare = 7;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 5);
			recipe.AddIngredient(ItemID.CobaltBar, 1);
			recipe.AddIngredient(ItemID.PixieDust, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 5);
			recipe.AddIngredient(ItemID.PalladiumBar, 1);
			recipe.AddIngredient(ItemID.PixieDust, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("Kraid_Head"));
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Kraid_Head"));
			Main.PlaySound(15,(int)player.position.X,(int)player.position.Y,0);
			return true;
		}
	}
}