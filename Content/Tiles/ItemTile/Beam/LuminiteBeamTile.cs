using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class LuminiteBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Luminite Beam");
			AddMapEntry(new Color(94, 229, 163), name);
			DustType = 1;
		}
	}
}
