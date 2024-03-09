using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class HiJumpBootsTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(224, 95, 3));
			//ItemDrop = ModContent.ItemType<Items.Accessories.HiJumpBoots>();
			DustType = 1;
		}
	}
}
