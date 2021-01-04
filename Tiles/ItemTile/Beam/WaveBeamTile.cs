using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class WaveBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wave Beam");
			AddMapEntry(new Color(224, 168, 224), name);
			drop = mod.ItemType("WaveBeamAddon");
			dustType = 1;
		}
	}
}