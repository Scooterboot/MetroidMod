using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class PowerGripTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Power Grip");
			AddMapEntry(new Color(164, 164, 46), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.PowerGrip>();
			DustType = 1;
		}
	}
}
