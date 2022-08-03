using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class NorfairBubbleZM : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<NorfairBubbleSM>()] = true;

			DustType = DustID.Grass;
			MinPick = 100;
			HitSound = SoundID.Drip;
			ItemDrop = ModContent.ItemType<Items.Tiles.NorfairBubbleZM>();

			AddMapEntry(new Color(64, 168, 192));
		}
	}
}
