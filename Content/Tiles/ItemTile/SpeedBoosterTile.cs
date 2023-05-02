using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class SpeedBoosterTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Speed Booster");
			AddMapEntry(new Color(98, 174, 129), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.SpeedBooster>();
			DustType = 1;
		}
	}
}
