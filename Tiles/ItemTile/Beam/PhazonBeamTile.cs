using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class PhazonBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Phazon Beam");
			AddMapEntry(new Color(72, 192, 248), name);
			drop = mod.ItemType("PhazonBeamAddon");
			dustType = 1;
		}
	}
}