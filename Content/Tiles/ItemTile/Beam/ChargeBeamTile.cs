using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class ChargeBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Charge Beam");
			AddMapEntry(new Color(104, 104, 128), name);
			DustType = 1;
		}
	}
}
