using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class SolarBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Solar Beam");
			AddMapEntry(new Color(254, 158, 35), name);
			drop = mod.ItemType("SolarBeamAddon");
			dustType = 1;
		}
	}
}