using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class GrappleBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Grapple Beam");
			AddMapEntry(new Color(121, 221, 139), name);
			ItemDrop = ModContent.ItemType<Items.Tools.GrappleBeam>();
			DustType = 1;
		}
	}
}
