using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class PlasmaBeamGreenTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Plasma Beam (Green)");
			AddMapEntry(new Color(90, 219, 16), name);
			DustType = 1;
		}
	}
}
