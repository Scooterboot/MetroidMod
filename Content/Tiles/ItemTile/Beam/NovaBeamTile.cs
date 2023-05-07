using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class NovaBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Nova Beam");
			AddMapEntry(new Color(1, 235, 15), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V2.NovaBeamAddon>();
			DustType = 1;
		}
	}
}
