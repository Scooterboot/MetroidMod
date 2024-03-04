using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class NightmareCoreX : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nightmare Core-X");
			/* Tooltip.SetDefault("Soft and squishy\n" + 
			"Contains gravity altering properties"); */
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 8));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 64;
			Item.height = 64;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<NightmareCoreXFragment>(20)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NightmareCoreXFragment", 20);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
