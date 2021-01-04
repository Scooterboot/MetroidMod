using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class SpazerTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Spazer");
			AddMapEntry(new Color(248, 176, 0), name);
			drop = mod.ItemType("SpazerAddon");
			dustType = 1;
		}
	}
}