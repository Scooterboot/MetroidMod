using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class ChargeBeamV2Tile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Charge Beam V2");
			AddMapEntry(new Color(248, 246, 110), name);
			drop = mod.ItemType("ChargeBeamV2Addon");
			dustType = 1;
		}
	}
}