using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class PlasmaBeamRedTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Plasma Beam (Red)");
			AddMapEntry(new Color(216, 0, 0), name);
			ItemDrop = ModContent.ItemType<Items.Addons.PlasmaBeamRedAddon>();
			DustType = 1;
		}
	}
}
