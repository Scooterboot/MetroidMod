using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class IceSpreader : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Spreader");
			AddMapEntry(new Color(42, 120, 213), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.IceSpreaderAddon>();
			DustType = 1;
		}
	}
}
