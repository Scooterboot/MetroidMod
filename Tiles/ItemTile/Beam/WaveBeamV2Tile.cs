using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class WaveBeamV2Tile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wave Beam V2");
			AddMapEntry(new Color(255, 126, 255), name);
			drop = mod.ItemType("WaveBeamV2Addon");
			dustType = 1;
		}
	}
}