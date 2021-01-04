using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class VortexBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Vortex Beam");
			AddMapEntry(new Color(0, 242, 170), name);
			drop = mod.ItemType("VortexBeamAddon");
			dustType = 1;
		}
	}
}