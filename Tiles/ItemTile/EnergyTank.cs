using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class EnergyTank : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Energy Tank");
			AddMapEntry(new Color(243, 178, 0), name);
			drop = mod.ItemType("EnergyTank");
			dustType = 1;
		}
	}
}