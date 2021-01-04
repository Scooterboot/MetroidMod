using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class IceBeamV2Tile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Beam V2");
			AddMapEntry(new Color(72, 192, 248), name);
			drop = mod.ItemType("IceBeamV2Addon");
			dustType = 1;
		}
	}
}