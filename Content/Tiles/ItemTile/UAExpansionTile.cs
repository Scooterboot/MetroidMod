using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class UAExpansionTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Missile Expansion");
			AddMapEntry(new Color(132, 4, 20), name);
			Main.tileOreFinderPriority[Type] = 805;
			DustType = 1;
			AnimationFrameHeight = 18;
			TileObjectData.newTile.LavaDeath = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileObsidianKill[Type] = false;
		}
	}
}
