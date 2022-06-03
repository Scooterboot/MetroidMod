using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile.Beam
{
	public class StardustBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Stardust Beam");
			AddMapEntry(new Color(35, 200, 254), name);
			ItemDrop = ModContent.ItemType<Items.Addons.V3.StardustBeamAddon>();
			DustType = 1;
		}
	}
}
