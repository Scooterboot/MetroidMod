using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class PhazonBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Phazon Beam");
			AddMapEntry(new Color(72, 192, 248), name);
			ItemDrop = ModContent.ItemType<Items.Addons.PhazonBeamAddon>();
			DustType = 1;
		}
	}
}
