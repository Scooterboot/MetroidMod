using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class IceBeamV2Tile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Beam V2");
			AddMapEntry(new Color(72, 192, 248), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V2.IceBeamV2Addon>();
			DustType = 1;
		}
	}
}
