using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class ScrewAttackTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AddMapEntry(new Color(255, 218, 98));
			//ItemDrop = ModContent.ItemType<Items.Accessories.ScrewAttack>();
			DustType = 1;
		}
	}
}
