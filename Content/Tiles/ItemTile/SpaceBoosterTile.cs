using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpaceBoosterTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(79, 188, 55));
			//ItemDrop = ModContent.ItemType<Items.Accessories.SpaceBooster>();
			DustType = 1;
		}
	}
}
