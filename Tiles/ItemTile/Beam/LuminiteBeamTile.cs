using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class LuminiteBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Luminite Beam");
			AddMapEntry(new Color(94, 229, 163), name);
			drop = mod.ItemType("LuminiteBeamAddon");
			dustType = 1;
		}
	}
}