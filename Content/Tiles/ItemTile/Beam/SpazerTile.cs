using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile.Beam
{
	public class SpazerTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Spazer");
			AddMapEntry(new Color(248, 176, 0), name);
			ItemDrop = ModContent.ItemType<Items.Addons.SpazerAddon>();
			DustType = 1;
		}
	}
}
