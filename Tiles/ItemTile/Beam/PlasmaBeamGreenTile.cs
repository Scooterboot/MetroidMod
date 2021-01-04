using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class PlasmaBeamGreenTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Plasma Beam (Green)");
			AddMapEntry(new Color(90, 219, 16), name);
			drop = mod.ItemType("PlasmaBeamGreenAddon");
			dustType = 1;
		}
	}
}