using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class PlasmaBeamRedTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Plasma Beam (Red)");
			AddMapEntry(new Color(216, 0, 0), name);
			drop = mod.ItemType("PlasmaBeamRedAddon");
			dustType = 1;
		}
	}
}