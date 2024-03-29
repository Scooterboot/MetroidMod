using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class ChargeBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Charge Beam");
			AddMapEntry(new Color(104, 104, 128), name);
			ItemDrop = ModContent.ItemType<Items.Addons.ChargeBeamAddon>();
			DustType = 1;
		}
	}
}
