using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Variants
{
	internal class YellowHatch : ModHatch
	{
		public override int ItemType => ModContent.ItemType<YellowHatchItem>();
		public override Color PrimaryColor => new(160, 0, 0);
		public override bool InteractableByDefault => false;
	}

	internal class YellowHatchItem : HatchItem
	{
		public override ModHatch Hatch => ModContent.GetInstance<YellowHatch>();
	}
}
