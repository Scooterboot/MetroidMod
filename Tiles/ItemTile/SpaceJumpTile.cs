using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class SpaceJumpTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Jump");
			AddMapEntry(new Color(252, 164, 12), name);
			drop = mod.ItemType("SpaceJump");
			dustType = 1;
		}
	}
}