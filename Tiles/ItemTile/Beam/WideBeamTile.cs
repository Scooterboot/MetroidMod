using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class WideBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wide Beam");
			AddMapEntry(new Color(247, 132, 227), name);
			drop = mod.ItemType("WideBeamAddon");
			dustType = 1;
		}
	}
}