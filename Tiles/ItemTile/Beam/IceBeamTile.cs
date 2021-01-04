using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class IceBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Beam");
			AddMapEntry(new Color(112, 146, 224), name);
			drop = mod.ItemType("IceBeamAddon");
			dustType = 1;
		}
	}
}