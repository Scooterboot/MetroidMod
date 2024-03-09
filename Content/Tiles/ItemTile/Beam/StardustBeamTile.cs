using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class StardustBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Stardust Beam");
			AddMapEntry(new Color(35, 200, 254), name);
			DustType = 1;
		}
	}
}
