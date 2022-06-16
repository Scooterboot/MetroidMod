using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class WaveBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wave Beam");
			AddMapEntry(new Color(224, 168, 224), name);
			ItemDrop = ModContent.ItemType<Items.Addons.WaveBeamAddon>();
			DustType = 1;
		}
	}
}
