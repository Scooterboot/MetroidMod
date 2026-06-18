using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class ScrewSpaceBoosterTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(250, 242, 88));
			DustType = DustID.Meteorite;
		}
	}
}
