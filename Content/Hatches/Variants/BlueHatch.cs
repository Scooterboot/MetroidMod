using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Variants
{
	internal class BlueHatch : ModHatch
	{
		public override int ItemType => ModContent.ItemType<BlueHatchItem>();
		public override Color PrimaryColor => new(56, 112, 224);
		public override bool InteractableByDefault => true;
	}

	internal class BlueHatchItem : HatchItem
	{
		public override ModHatch Hatch => ModContent.GetInstance<BlueHatch>();
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Sapphire)
				.AddRecipeGroup("IronBar", 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
