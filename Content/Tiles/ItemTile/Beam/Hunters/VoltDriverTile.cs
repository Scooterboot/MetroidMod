using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam.Hunters
{
	public class VoltDriverTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("VoltDriver");
			AddMapEntry(new Color(255, 126, 255), name);
			ItemDrop = ModContent.ItemType<Items.Addons.Hunters.VoltDriverAddon>();
			DustType = 1;
		}
	}
}
