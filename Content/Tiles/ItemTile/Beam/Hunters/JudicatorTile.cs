using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam.Hunters
{
	public class JudicatorTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Judicator");
			AddMapEntry(new Color(255, 126, 255), name);
			ItemDrop = ModContent.ItemType<Items.Addons.Hunters.JudicatorAddon>();
			DustType = 1;
		}
	}
}
