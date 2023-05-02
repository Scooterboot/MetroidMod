using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpaceJumpTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Jump Boots");
			AddMapEntry(new Color(254, 159, 25), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.SpaceJump>();
			DustType = 1;
		}
	}
}
