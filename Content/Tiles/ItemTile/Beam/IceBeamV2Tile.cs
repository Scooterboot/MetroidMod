using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class IceBeamV2Tile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Ice Beam V2");
			AddMapEntry(new Color(72, 192, 248), name);
			DustType = 1;
		}
	}
}
