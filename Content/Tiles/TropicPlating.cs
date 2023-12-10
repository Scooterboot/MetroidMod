using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class TropicPlating1 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			//ItemDrop= ModContent.ItemType<Items.Tiles.TropicPlating1>();

			AddMapEntry(new Color(189, 206, 181));
		}
	}
	public class TropicPlating2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			//ItemDrop= ModContent.ItemType<Items.Tiles.TropicPlating2>();

			AddMapEntry(new Color(189, 206, 181));
		}
	}
	public class TropicPlating3 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			//ItemDrop= ModContent.ItemType<Items.Tiles.TropicPlating3>();

			AddMapEntry(new Color(181, 224, 74));
		}
	}
	public class TropicPlating4 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			//ItemDrop= ModContent.ItemType<Items.Tiles.TropicPlating4>();

			AddMapEntry(new Color(103, 121, 30));
		}
	}
	public class TropicPlating5 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			//ItemDrop= ModContent.ItemType<Items.Tiles.TropicPlating5>();

			AddMapEntry(new Color(103, 121, 30));
		}
	}
}
