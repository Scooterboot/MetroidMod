using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class NovaBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Nova Beam");
			AddMapEntry(new Color(1, 235, 15), name);
			DustType = 1;
		}
	}
}
