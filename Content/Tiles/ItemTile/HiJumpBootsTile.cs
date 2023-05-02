using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class HiJumpBootsTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hi-Jump Boots");
			AddMapEntry(new Color(224, 95, 3), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.HiJumpBoots>();
			DustType = 1;
		}
	}
}
