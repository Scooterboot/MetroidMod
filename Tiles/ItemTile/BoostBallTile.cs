using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class BoostBallTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Boost Ball");
			AddMapEntry(new Color(221, 143, 0), name);
			drop = mod.ItemType("BoostBallAddon");
			dustType = 1;
		}
	}
}