using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpeedBoosterTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(98, 174, 129));
			//ItemDrop = ModContent.ItemType<Items.Accessories.SpeedBooster>();
			DustType = 1;
		}
	}
}
