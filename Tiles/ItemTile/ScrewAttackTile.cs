using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class ScrewAttackTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Screw Attack");
			AddMapEntry(new Color(252, 244, 100), name);
			drop = mod.ItemType("ScrewAttack");
			dustType = 1;
		}
	}
}