using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class WideBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wide Beam");
			AddMapEntry(new Color(247, 132, 227), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V2.WideBeamAddon>();
			DustType = 1;
		}
	}
}
