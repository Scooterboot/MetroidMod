using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpaceBoosterTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Booster");
			AddMapEntry(new Color(79, 188, 55), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.SpaceBooster>();
			DustType = 1;
		}
	}
}
