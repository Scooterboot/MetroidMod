using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile.Beam
{
	public class PhazonBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Phazon Beam");
			AddMapEntry(new Color(72, 192, 248), name);
			ItemDrop = ModContent.ItemType<Items.Addons.PhazonBeamAddon>();
			DustType = 1;
		}
	}
}
