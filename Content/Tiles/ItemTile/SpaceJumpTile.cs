using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpaceJumpTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(254, 159, 25));
			//ItemDrop = ModContent.ItemType<Items.Accessories.SpaceJump>();
			DustType = 1;
		}
	}
}
