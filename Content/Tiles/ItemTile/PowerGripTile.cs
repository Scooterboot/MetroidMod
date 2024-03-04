using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class PowerGripTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(164, 164, 46));
			//ItemDrop = ModContent.ItemType<Items.Accessories.PowerGrip>();
			DustType = 1;
		}
	}
}
