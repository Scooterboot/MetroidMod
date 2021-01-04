using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class XRayScopeTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("X-Ray Scope");
			AddMapEntry(new Color(195, 255, 82), name);
			drop = mod.ItemType("XRayScope");
			dustType = 1;
		}
	}
}