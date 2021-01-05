#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

#endregion

namespace MetroidMod.Tiles
{
	public class PhazonCore : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<PhazonTile>()] = true;

			dustType = 28;
			minPick = 1000;//215;
			soundType = SoundID.Tink;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Phazon Core");
			AddMapEntry(new Color(255, 65, 0), name);
		}

		public override bool CanExplode(int x, int y) => false;

		public override void NumDust(int x, int y, bool fail, ref int num) => num = fail ? 1 : 3;
		
		public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
		{	
			r = (255f/255f);
			g = (105f/255f);
			b = 0f;
		}

		public override void KillTile(int x, int y, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			if(!fail && !effectOnly)
			{
				for(int i = x - 16; i < x + 16; i++)
				{
					for(int j = y - 16; j < y + 16; j++)
					{
						if(Main.tile[i,j].type == ModContent.TileType<PhazonCore>())
						{
							for (int k = 0; k < 10; k++)
							{
								int dustID = Dust.NewDust(new Vector2((x * 16) - 11, (y * 16) - 11), 22, 22, 68, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, 100, Color.White, 7f);
								Main.dust[dustID].noGravity = true;
							}
							Main.tile[i, j].active(false);
							Projectile.NewProjectile(x * 16, y * 16, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, ModContent.ProjectileType<Projectiles.PhazonExplosion>(), 20, 0.1f, Main.myPlayer);
						}
						WorldGen.SquareTileFrame(i, j);
					}
				}

				for (int i = 0; i < 10; i++)
				{
					int dustID = Dust.NewDust(new Vector2((x * 16) - 11, (y * 16) - 11), 22, 22, 68, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, 100, Color.White, 7f);
					Main.dust[dustID].noGravity = true;
				}

				Projectile.NewProjectile(x * 16, y * 16, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, ModContent.ProjectileType<Projectiles.PhazonExplosion>(), 0, 0.1f, Main.myPlayer);

				Main.PlaySound(SoundID.Item, x * 16, y * 16, 14);
				Main.NewText("Your world has been corrupt with Phazon!", 50, 125, 255, false);

				int Amount_Of_Spawns = 100 + (int)(Main.maxTilesY * 0.2f);
				for(int i = 0; i < Amount_Of_Spawns; i++)
				{
					Common.Worlds.MWorld.AddPhazon();
				}
			}
		}
	}
}