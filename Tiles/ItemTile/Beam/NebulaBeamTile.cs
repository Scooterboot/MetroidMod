using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class NebulaBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Nebula Beam");
			AddMapEntry(new Color(254, 126, 229), name);
			drop = mod.ItemType("NebulaBeamAddon");
			dustType = 1;
		}
	}
}