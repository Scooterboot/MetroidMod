using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class NorfairBrick : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 100;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(168, 104, 87));
		}
	}
}
