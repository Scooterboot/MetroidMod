using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class SuperMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Super Missile");
			AddMapEntry(new Color(15, 192, 39), name);
			DustType = 1;
		}
	}
}
