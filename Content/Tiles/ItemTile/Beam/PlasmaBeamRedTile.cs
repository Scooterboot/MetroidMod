using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class PlasmaBeamRedTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Plasma Beam (Red)");
			AddMapEntry(new Color(216, 0, 0), name);
			DustType = 1;
		}
	}
}
