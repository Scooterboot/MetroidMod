using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class SpiderBallTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Spider Ball");
			AddMapEntry(new Color(15, 186, 0), name);
			drop = mod.ItemType("SpiderBallAddon");
			dustType = 1;
		}
	}
}