using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile.Beam
{
	public class IceBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Beam");
			AddMapEntry(new Color(112, 146, 224), name);
			ItemDrop = ModContent.ItemType<Items.Addons.IceBeamAddon>();
			DustType = 1;
		}
	}
}
