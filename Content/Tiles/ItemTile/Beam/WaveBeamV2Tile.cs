using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile.Beam
{
	public class WaveBeamV2Tile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wave Beam V2");
			AddMapEntry(new Color(255, 126, 255), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V2.WaveBeamV2Addon>();
			DustType = 1;
		}
	}
}
