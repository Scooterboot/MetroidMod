using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace MetroidMod.Tiles
{
	public class TEBlueHatch : ModTileEntity
	{
		internal bool open = false;
		private int timer = 0;
		public override void Update()
		{

		
			int i = Position.X;
			int j = Position.Y;
			int x = i - (Main.tile[i, j].frameX / 18) % 3;
			int y = j - (Main.tile[i, j].frameY / 18) % 3;
			
				 for (int l = 0; l < 200; l++)
            {
				Projectile p = Main.projectile[l];
				float px = Math.Abs((p.Center.X / 16) - (x +1.5f));
				float py = Math.Abs((p.Center.Y / 16) - (y +1.5f));
				
				if (p.friendly && p.active && px < (2.5f + p.width/32) && py < (1.5f + p.width/32))
				{
					//Main.NewText("px is " + px);
					//Main.NewText("py is " + py);
					open = true;
				}
            }
			
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
			if (open)
			{
				timer++;
				if (timer < 2)
				{
					Main.PlaySound(SoundLoader.customSoundType, x * 16, y * 16,  mod.GetSoundSlot(SoundType.Custom, "Sounds/HatchOpenSound"));
					for (int l = x; l < x + 3; l++)
					{
						for (int m = y; m < y + 3; m++)
						{
							if (Main.tile[l, m] == null)
							{
								Main.tile[l, m] = new Tile();
							}
							if (Main.tile[l, m].active() && Main.tile[l, m].type == (ushort)mod.TileType("BlueHatch"))
							{
								Main.tile[l,m].type = (ushort)mod.TileType("BlueHatchOpen");
							}
						}
					}
				}
			}
			else
			{
				timer = 0;
			}
for (int l = x; l < x + 3; l++)
				{
					for (int m = y; m < y + 3; m++)
					{
						if (Main.tile[l, m] == null)
						{
							Main.tile[l, m] = new Tile();
						}
						if (Main.tile[l, m].active() && Main.tile[l, m].type == (ushort)mod.TileType("BlueHatchOpen"))
						{
							open = true;
						}
						if (Main.tile[l, m].active() && Main.tile[l, m].type == (ushort)mod.TileType("BlueHatch"))
						{
							open = false;
						}
					}
				}
			if (timer > 900)
			{
				open = false;
				Main.PlaySound(SoundLoader.customSoundType, x * 16, y * 16,  mod.GetSoundSlot(SoundType.Custom, "Sounds/HatchCloseSound"));
					for (int l = x; l < x + 3; l++)
				{
					for (int m = y; m < y + 3; m++)
					{
						if (Main.tile[l, m] == null)
						{
							Main.tile[l, m] = new Tile();
						}
						if (Main.tile[l, m].active() && Main.tile[l, m].type == (ushort)mod.TileType("BlueHatchOpen"))
						{
							Main.tile[l,m].type = (ushort)mod.TileType("BlueHatch");
						}
					}
				}
			}
		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active() && (tile.type == mod.TileType("BlueHatch") || tile.type == mod.TileType("BlueHatchOpen")) && tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3); 
				NetMessage.SendData(87, -1, -1, "", i - 1, j - 2, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i - 1, j - 2);
		}
	}
}