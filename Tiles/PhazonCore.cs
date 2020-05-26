using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Tiles
{
	public class PhazonCore : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			//Main.tileMergeDirt[Type] = true;
			Main.tileMerge[Type][mod.TileType("PhazonTile")] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			soundType = 21;
			dustType = 28;
			minPick = 1000;//215;
			//drop = mod.ItemType("Phazon");
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Phazon Core");
			AddMapEntry(new Color(255, 65, 0), name);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
		{	
			r = (255f/255f);
			g = (105f/255f);
			b = 0f;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillTile(int x, int y, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if(!fail && !effectOnly)
			{
				for(int i = x - 16; i < x + 16; i++)
				{
					for(int j = y - 16; j < y + 16; j++)
					{
						if(Main.tile[i,j].type == mod.TileType("PhazonCore"))
						{
							Main.tile[i,j].active(false);
							for (int k = 0; k < 10; k++)
							{
								int dustID = Dust.NewDust(new Vector2((x*16)-11,(y*16)-11), 22, 22, 68, Main.rand.Next(10)-5, Main.rand.Next(10)-5, 100, Color.White, 7f);
								Main.dust[dustID].noGravity = true;
							}
							int num53 = Projectile.NewProjectile(x * 16, y * 16,Main.rand.Next(10)-5,Main.rand.Next(10)-5,mod.ProjectileType("PhazonExplosion"),20,0.1f,Main.myPlayer);
						}
						WorldGen.SquareTileFrame(i, j, true);
					}
				}
				for (int i = 0; i < 10; i++)
				{
					int dustID = Dust.NewDust(new Vector2((x*16)-11,(y*16)-11), 22, 22, 68, Main.rand.Next(10)-5, Main.rand.Next(10)-5, 100, Color.White, 7f);
					Main.dust[dustID].noGravity = true;
				}
				int num54 = Projectile.NewProjectile(x * 16, y * 16,Main.rand.Next(10)-5,Main.rand.Next(10)-5,mod.ProjectileType("PhazonExplosion"),20,0.1f,Main.myPlayer);
				Main.PlaySound(2, x * 16, y * 16, 14);
				Main.NewText("Your world has been corrupt with Phazon!", 50, 125, 255, false);
				int Amount_Of_Spawns = 100+(int)(Main.maxTilesY/5);
				for(int p = 0; p < Amount_Of_Spawns; p++)
				{
					MWorld.AddPhazon();
				}
			}
		}
	}
}