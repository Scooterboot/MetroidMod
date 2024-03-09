using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class SeekerMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Seeker Missile");
			AddMapEntry(new Color(173, 0, 82), name);
			DustType = 1;
		}
	}
}
