using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class IceSpreader : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Ice Spreader");
			AddMapEntry(new Color(42, 120, 213), name);
			DustType = 1;
		}
	}
}
