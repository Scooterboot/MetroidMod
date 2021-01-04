using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class NovaBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Nova Beam");
			AddMapEntry(new Color(1, 235, 15), name);
			drop = mod.ItemType("NovaBeamAddon");
			dustType = 1;
		}
	}
}