using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile.Beam
{
	public class NebulaBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Nebula Beam");
			AddMapEntry(new Color(254, 126, 229), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V3.NebulaBeamAddon>();
			DustType = 1;
		}
	}
}
