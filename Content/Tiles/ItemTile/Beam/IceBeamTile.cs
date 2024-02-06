using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class IceBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Ice Beam");
			AddMapEntry(new Color(112, 146, 224), name);
			DustType = 1;
		}
	}
}
