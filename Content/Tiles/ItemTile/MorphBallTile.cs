using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile
{
	public class MorphBallTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Morph Ball");
			AddMapEntry(new Color(250, 85, 34), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.MorphBall>();
			DustType = 1;
		}
	}
}
