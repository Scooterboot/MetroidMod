using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class ChargeBeamV2Tile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Charge Beam V2");
			AddMapEntry(new Color(248, 246, 110), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V2.ChargeBeamV2Addon>();
			DustType = 1;
		}
	}
}
