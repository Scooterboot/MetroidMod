using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class PhazonExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon");
		}
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile P = Projectile;
			P.rotation = (float)Math.Atan2(P.velocity.Y, P.velocity.X) + MathHelper.PiOver2;
			P.tileCollide = false;
			P.alpha = 255;
			P.localAI[0] += 1f;
			if (P.localAI[0] > 1f)
			{
				for (int l = 0; l < 4; l++)
				{
					float x = P.position.X - (P.velocity.X * (l * 0.25f)) + (P.width / 2);
					float y = P.position.Y - (P.velocity.Y * (l * 0.25f)) + (P.height / 2);
					int num20 = Dust.NewDust(new Vector2(x, y), 1, 1, 68, 0f, 0f, 100, default(Color), Main.rand.Next(3, 6));
					Main.dust[num20].position.X = x;
					Main.dust[num20].position.Y = y;
					Main.dust[num20].velocity *= 0.2f;
					Main.dust[num20].noGravity = true;
				}
			}
			if (P.localAI[0] < 60f)
			{
				P.velocity.X += Main.rand.Next(-50, 51) * 0.075f;
				P.velocity.Y += Main.rand.Next(-50, 51) * 0.075f;
			}
			else if (P.velocity.Length() < 16)
			{
				P.velocity *= 1.1f;
			}
			//Convert((int)(Projectile.position.X + (Projectile.width * 0.5f)) / 16, (int)(Projectile.position.Y + (Projectile.height * 0.5f)) / 16, 2);
		}
		//
		private static void Convert(int i, int j, int size = 4)
		{
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt((size * size) + (size * size)))
					{
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;


						// Convert all walls to ExampleWall (or ExampleWallUnsafe for SpiderUnsafe)
						if (wall != 0 && wall != ModContent.WallType<Walls.PhazestoneWall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.PhazestoneWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}

						// If the tile is stone, convert to ExampleBlock
						if (TileID.Sets.Conversion.Stone[type])
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PhazestoneTile>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						if (TileID.Sets.Conversion.MushroomGrass[type] || TileID.Sets.Conversion.Grass[type] || TileID.Sets.Conversion.JungleGrass[type])
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PhazonGrass>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == TileID.Trees)
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PhazonTreeTile>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
/*						else if (type == TileID.Plants || type == TileID.Plants2)
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PhazonTreeTile>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == TileID.Vines)
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PhazonTreeTile>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}*/
					}
				}
			}
		}
	}
}
