using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class ChargeBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Charge Beam");
			AddMapEntry(new Color(104, 104, 128), name);
			drop = mod.ItemType("ChargeBeamAddon");
			dustType = 1;
		}
	}
}