using Microsoft.Xna.Framework;
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
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Tiles/BlueHatch";
		public override ModHatch Hatch => ModContent.GetInstance<BlueHatch>();
	}
}
