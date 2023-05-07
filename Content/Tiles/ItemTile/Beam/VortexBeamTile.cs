using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class VortexBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Vortex Beam");
			AddMapEntry(new Color(0, 242, 170), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V3.VortexBeamAddon>();
			DustType = 1;
		}
	}
}
