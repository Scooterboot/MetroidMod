using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpinBoostTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(254, 159, 25));
			//ItemDrop = ModContent.ItemType<Items.Accessories.SpinBoost>();
			DustType = 1;
		}
	}
}
