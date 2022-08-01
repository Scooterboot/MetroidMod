using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class GreenBrinstarRootsTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileLargeFrames[Type] = 1;

			DustType = DustID.Grass;
			MinPick = 65;
			HitSound = SoundID.Grass;
			ItemDrop = ModContent.ItemType<Items.Tiles.GreenBrinstarRootsTile>();

			AddMapEntry(new Color(43, 74, 36));
		}
	}
}
