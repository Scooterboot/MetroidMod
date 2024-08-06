using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Variants
{
	internal class GreenHatch : ModHatch
	{
		public override int ItemType => ModContent.ItemType<GreenHatchItem>();
		public override Color PrimaryColor => new(0, 160, 0);
		public override bool InteractableByDefault => false;
	}

	internal class GreenHatchItem : HatchItem
	{
		public override ModHatch Hatch => ModContent.GetInstance<GreenHatch>();
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.AdamantiteBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe(20)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.TitaniumBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
