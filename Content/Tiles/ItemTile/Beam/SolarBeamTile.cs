using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class SolarBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Solar Beam");
			AddMapEntry(new Color(254, 158, 35), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V3.SolarBeamAddon>();
			DustType = 1;
		}
	}
}
