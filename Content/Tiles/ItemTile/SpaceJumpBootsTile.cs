using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpaceJumpBootsTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(74, 140, 188));
			//ItemDrop = ModContent.ItemType<Items.Accessories.SpaceJumpBoots>();
			DustType = 1;
		}
	}
}
