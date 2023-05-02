using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class ScrewAttackTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Screw Attack");
			AddMapEntry(new Color(255, 218, 98), name);
			ItemDrop = ModContent.ItemType<Items.Accessories.ScrewAttack>();
			DustType = 1;
		}
	}
}
