using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class MorphBallTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Morph Ball");
			AddMapEntry(new Color(250, 85, 34), name);
			drop = mod.ItemType("MorphBall");
			dustType = 1;
		}
	}
}