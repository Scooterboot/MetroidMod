using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam.Hunters
{
	public class ImperialistTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Imperialist");
			AddMapEntry(new Color(255, 126, 255), name);
			ItemDrop = ModContent.ItemType<Items.Addons.Hunters.ImperialistAddon>();
			DustType = 1;
		}
	}
}
