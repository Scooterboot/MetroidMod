using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class SpaceJumpBootsTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Jump Boots");
			AddMapEntry(new Color(71, 196, 144), name);
			drop = mod.ItemType("SpaceJumpBoots");
			dustType = 1;
		}
	}
}