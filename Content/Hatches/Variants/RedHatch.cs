using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Variants
{
	internal class RedHatch : ModHatch
	{
		public override int ItemType => ModContent.ItemType<RedHatchItem>();
		public override Color PrimaryColor => new(160, 0, 0);
		public override bool InteractableByDefault => false;
	}

	internal class RedHatchItem : HatchItem
	{
		public override ModHatch Hatch => ModContent.GetInstance<RedHatch>();
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Ruby)
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
