using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class HiJumpBootsTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hi-Jump Boots");
			AddMapEntry(new Color(71, 196, 144), name);
			drop = mod.ItemType("HiJumpBoots");
			dustType = 1;
		}
	}
}