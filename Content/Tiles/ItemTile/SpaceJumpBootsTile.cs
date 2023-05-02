using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpaceJumpBootsTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Jump Boots");
			AddMapEntry(new Color(74, 140, 188), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.SpaceJumpBoots>();
			DustType = 1;
		}
	}
}
