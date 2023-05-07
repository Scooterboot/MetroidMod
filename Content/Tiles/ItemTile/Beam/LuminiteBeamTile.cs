using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class LuminiteBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Luminite Beam");
			AddMapEntry(new Color(94, 229, 163), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V3.LuminiteBeamAddon>();
			DustType = 1;
		}
	}
}
