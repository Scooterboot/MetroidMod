using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class NebulaMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Nebula Missile");
			AddMapEntry(new Color(166, 50, 150), name);
			DustType = 1;
		}
	}
}
