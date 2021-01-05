#region Using directives

using System;
using System.IO;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace MetroidMod.Common.Worlds
{
	[Flags]
	public enum MetroidBossDown
	{
		downedNone = 0,
		downedTorizo = 1<<0,
		downedSerris = 1<<1,
		downedKraid = 1<<2,
		downedPhantoon = 1<<3,
		downedNightmare = 1<<4,
		downedOmegaPirate = 1<<5,
		downedGoldenTorizo = 1<<6
	}

    public partial class MWorld : ModWorld
    {
		public static MetroidBossDown bossesDown;
		public static Rectangle TorizoRoomLocation = new Rectangle(0,0,80,40);
		
        public static ushort[,] mBlockType = new ushort[Main.maxTilesX, Main.maxTilesY];
		
		public override void Initialize()
		{
			bossesDown = MetroidBossDown.downedNone;
		}

		public override TagCompound Save()
		{
			return new TagCompound {
				{"downed", (int)bossesDown},
				{"spawnedPhazonMeteor", spawnedPhazonMeteor},
				{"TorizoRoomLocation.X", TorizoRoomLocation.X},
				{"TorizoRoomLocation.Y", TorizoRoomLocation.Y}
			};
		}

		public override void Load(TagCompound tag)
		{
			int downed = tag.GetAsInt("downed");
			bossesDown = (MetroidBossDown)downed;
			spawnedPhazonMeteor = tag.Get<bool>("spawnedPhazonMeteor");
			
			TorizoRoomLocation.X = tag.Get<int>("TorizoRoomLocation.X");
			TorizoRoomLocation.Y = tag.Get<int>("TorizoRoomLocation.Y");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				bossesDown = (MetroidBossDown)reader.ReadInt32();
				spawnedPhazonMeteor = reader.ReadBoolean();
				TorizoRoomLocation.X = reader.ReadInt32();
				TorizoRoomLocation.Y = reader.ReadInt32();
			}
			else
			{
				ErrorLogger.Log("MetroidMod: Unknown loadVersion: " + loadVersion);
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((int)bossesDown);
			writer.Write(spawnedPhazonMeteor);
			writer.Write(TorizoRoomLocation.X);
			writer.Write(TorizoRoomLocation.Y);
		}

		public override void NetReceive(BinaryReader reader)
		{
			bossesDown = (MetroidBossDown)reader.ReadInt32();
			spawnedPhazonMeteor = reader.ReadBoolean();
			TorizoRoomLocation.X = reader.ReadInt32();
			TorizoRoomLocation.Y = reader.ReadInt32();
		}
		
		public override void PostDrawTiles()
		{
		    SpriteBatch spriteBatch = Main.spriteBatch;
		    Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		    if (Main.drawToScreen)
		    {
			zero = Vector2.Zero;
		    }
		    int x1 = (int)((Main.screenPosition.X) / 16f - 1f);
		    int x2 = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
		    int y1 = (int)((Main.screenPosition.Y) / 16f - 1f);
		    int y2 = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 5;
		    if (x1 < 0)
		    {
			x1 = 0;
		    }
		    if (x2 > Main.maxTilesX)
		    {
			x2 = Main.maxTilesX;
		    }
		    if (y1 < 0)
		    {
			y1 = 0;
		    }
		    if (y2 > Main.maxTilesY)
		    {
			y2 = Main.maxTilesY;
		    }
		    for (int i = x1; i < x2; i++)
		    {
			for (int j = y1; j < y2; j++)
			{
			    Tile tile = Main.tile[i, j];
			    Color color = Lighting.GetColor(i, j);
			    if (!Main.tile[i, j].active() || Main.tile[i, j].inActive() || !Main.tileSolid[Main.tile[i,j].type])
			    {
				color *= 0.5f;
			    }
			    bool draw = Terraria.GameContent.UI.WiresUI.Settings.DrawWires;
			    float scale = 1f; 
			    Vector2 screenPos = Main.screenPosition;
			    int xOff = -4 * 16;
			    int yOff = -4 * 16;
			    Vector2 drawPos = new Vector2((float)(i * 16 + xOff - (int)screenPos.X), (float)(j * 16 + yOff - (int)screenPos.Y)) + zero;

			    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.GameViewMatrix.ZoomMatrix);

			    if (draw)
			    {
				if (mBlockType[i, j] == 1)
				{
				    spriteBatch.Draw(mod.GetTexture("Tiles/CrumbleBlock"), drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
				}
			    }

			    spriteBatch.End();
			}
		    }
		}
		
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			int PotsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
			if (ShiniesIndex != -1)
			{
				tasks.Insert(ShiniesIndex + 1, new PassLegacy("Chozite Ore", delegate (GenerationProgress progress)
				{
					progress.Message = "Generating Chozite Ore";

					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 9E-05); k++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY), (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(4, 7), mod.TileType("ChoziteOreTile"), false, 0f, 0f, false, true);
					}
				}));
			}
			if (PotsIndex != -1)
			{
				tasks.Insert(PotsIndex - 3, new PassLegacy("Chozo Statues", delegate (GenerationProgress progress)
				{
					progress.Message = "Placing Chozo Statues";
					for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05); i++)
					{
						float num2 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05));
						bool flag = false;
						int num3 = 0;
						while (!flag)
						{
							if (AddChozoStatue(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 100)))
							{
								flag = true;
							}
							else
							{
								num3++;
								if (num3 >= 10000)
								{
									flag = true;
								}
							}
						}
						
					}
				}));
				tasks.Insert(PotsIndex - 2, new PassLegacy("Missile Expansions", delegate (GenerationProgress progress)
				{
					progress.Message = "Placing Missile Expansions";
					for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 15E-06); i++)
					{
						float num2 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 15E-06));
						bool flag = false;
						int num3 = 0;
						while (!flag)
						{
							if (AddExpansion(WorldGen.genRand.Next(1, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 100)))
							{
								flag = true;
							}
							else
							{
								num3++;
								if (num3 >= 10000)
								{
									flag = true;
								}
							}
						}
						
					}
				}));
				
				tasks.Insert(PotsIndex - 1, new PassLegacy("Chozo Ruins", ChozoRuins));
			}
		}
		
		public static ushort StatueItem(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			
			bool dungeon = Main.wallDungeon[(int)Main.tile[i, j].wall];
			bool jungle = (i >= Main.maxTilesX * 0.2 && i <= Main.maxTilesX * 0.35);
			if (WorldGen.dEnteranceX < Main.maxTilesX/2)
			{
				jungle = (i >= Main.maxTilesX * 0.65 && i <= Main.maxTilesX * 0.8);
			}
			
			ushort item = (ushort)mod.TileType("MorphBallTile");
			if(dungeon)
			{
				item = (ushort)mod.TileType("IceBeamTile");
			}
			else if(jungle && WorldGen.genRand.Next(10) <= 5)
			{
				item = (ushort)mod.TileType("SpazerTile");
			}
			else
			{
				int baseX = Main.maxTilesX / 2;
				int baseY = (int)WorldGen.rockLayer;
				float dist = (float)((Math.Abs(i - baseX) / (Main.maxTilesX/2)) + (Math.Max(j - baseY,0) / (Main.maxTilesY-WorldGen.rockLayer))) / 2;
				
				int rand = WorldGen.genRand.Next((int)Math.Max(100 * (1 - dist),5));
				if(rand < 1)
				{
					item = (ushort)mod.TileType("SpiderBallTile");
				}
				else if(rand < 3)
				{
					item = (ushort)mod.TileType("XRayScopeTile");
				}
				else if(rand < 8)
				{
					item = (ushort)mod.TileType("HiJumpBootsTile");
				}
				else if(rand < 13)
				{
					item = (ushort)mod.TileType("BoostBallTile");
				}
				else if(rand < 25)
				{
					item = (ushort)mod.TileType("WaveBeamTile");
				}
				else if(rand < 40)
				{
					item = (ushort)mod.TileType("BombTile");
				}
				else if(rand < 60)
				{
					item = (ushort)mod.TileType("ChargeBeamTile");
				}
				else
				{
					item = (ushort)mod.TileType("MorphBallTile");
				}
			}
			return item;
		}
		public static bool AddChozoStatue(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			
			int k = j;
			while (k < Main.maxTilesY)
			{
				int num2 = 0;
				ushort type = TileID.Stone;
				for(int l = 0; l < 3; l++)
				{
					if(Main.tile[i+l, k].active() && Main.tileSolid[(int)Main.tile[i+l, k].type])
					{
						num2++;
						type = Main.tile[i+l, k].type;
					}
				}
				if (num2 >= 2)// && !Main.tile[i, k - 1].active() && !Main.tile[i, k - 2].active() && !Main.tile[i, k - 3].active() && !Main.tile[i + 1, k - 1].active() && !Main.tile[i + 1, k - 2].active() && !Main.tile[i + 1, k - 3].active() && !Main.tile[i + 2, k - 1].active() && !Main.tile[i + 2, k - 2].active() && !Main.tile[i + 2, k - 3].active())
				{
					int num = k - 1;
					if (Main.tile[i, num - 1].lava() || Main.tile[i + 1, num - 1].lava() || Main.tile[i + 2, num - 1].lava())
					{
						return false;
					}
					if (!WorldGen.EmptyTileCheck(i, i+2, num-2, num, -1))
					{
						return false;
					}
					if(Main.tile[i, num].wall == WallID.LihzahrdBrickUnsafe)
					{
						return false;
					}
					
					int rand = WorldGen.genRand.Next(4); // 0 & 1 = statue only, 2 = statue + brick base, 3 = statue + brick shrine
					if(Main.wallDungeon[(int)Main.tile[i, num].wall])
					{
						rand = 0;
					}
					
					if(rand <= 1)
					{
						for (int tx = i; tx < i + 3; tx++)
						{
							Main.tile[tx, k].slope(0);
							Main.tile[tx, k].halfBrick(false);
							if(!Main.tile[tx, k].active())
							{
								Main.tile[tx, k].active(true);
								Main.tile[tx, k].type = type;
							}
						}
					}
					else
					{
						if(rand == 3)
						{
							for (int wx = i - 1; wx < i + 4; wx++)
							{
								for (int wy = k - 4; wy < k; wy++)
								{
									WorldGen.KillTile(wx, wy);
									
									if(WorldGen.genRand.Next(4) > 0)
									{
										WorldGen.KillWall(wx, wy);
										WorldGen.PlaceWall(wx, wy, WallID.Cave4Unsafe);
									}
								}
							}
							for (int tx = i - 2; tx < i + 5; tx++)
							{
								Main.tile[tx, k].slope(0);	
								Main.tile[tx, k].halfBrick(false);
								Main.tile[tx, k].active(true);	
								Main.tile[tx, k].type = 38;
								Main.tile[tx, k - 5].slope(0);	
								Main.tile[tx, k - 5].halfBrick(false);	
								Main.tile[tx, k - 5].active(true);	
								Main.tile[tx, k - 5].type = 38;
							}
							Main.tile[i - 2, k - 4].slope(0);
							Main.tile[i - 2, k - 4].halfBrick(false);
							Main.tile[i - 2, k - 4].active(true);
							Main.tile[i - 2, k - 4].type = 38;

							Main.tile[i + 4, k - 4].slope(0);
							Main.tile[i + 4, k - 4].halfBrick(false);
							Main.tile[i + 4, k - 4].active(true);
							Main.tile[i + 4, k - 4].type = 38;
						}
						for (int tx2 = i - 1; tx2 < i + 4; tx2++)
						{
							Main.tile[tx2, k].slope(0);
							Main.tile[tx2, k].halfBrick(false);
							Main.tile[tx2, k].active(true);
							Main.tile[tx2, k].type = 38;
							if(rand == 3)
							{
								Main.tile[tx2, k - 6].slope(0);
								Main.tile[tx2, k - 6].halfBrick(false);
								Main.tile[tx2, k - 6].active(true);
								Main.tile[tx2, k - 6].type = 38;
							}
						}
					}
					
					int dir = 1;
					if(WorldGen.genRand.Next(2) == 0)
					{
						dir = -1;
					}
					
					int statueX = i+2;
					int statueX2 = i;
					if(dir == 1)
					{
						statueX = i+1;
						statueX2 = i+2;
					}
					int statueY = num;
					
					WorldGen.PlaceObject(statueX, statueY, mod.TileType("ChozoStatueNatural"), false, 0, 0, -1, -dir);
					WorldGen.PlaceObject(statueX2, statueY, mod.TileType("ChozoStatueArmNatural"), false, 0, 0, -1, -dir);
					
					ushort item = StatueItem(statueX2,statueY-2);
					WorldGen.PlaceObject(statueX2, statueY-2, item);
					
					return true;
				}
				else
				{
					k++;
				}
			}
			return false;
		}
		public static bool AddExpansion(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			int k = j;
			while (k < Main.maxTilesY)
			{
				if (Main.tile[i, k].active() && Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tile[i, k - 1].active())
				{
					int num = k - 1;
					if (Main.tile[i, num].lava() || Main.tile[i, num - 1].lava())
					{
						return false;
					}
					Main.tile[i, k].slope(0);
					Main.tile[i, k].halfBrick(false);
					Main.tile[i, num].active(true);
					Main.tile[i, num].type = (ushort)mod.TileType("MissileExpansionTile");
					Main.tile[i, num].frameX = 0;
					Main.tile[i, num].frameY = 0;
					return true;
				}
				else
				{
					k++;
				}
			}
			return false;
		}
		
		private void ChozoRuins(GenerationProgress progress)
		{
			Rectangle uDesert = WorldGen.UndergroundDesertLocation;
			
			progress.Message = "Chozo Ruins...Determining X Position";
			
			int ruinsWidth = WorldGen.genRand.Next(50, 60);
			int ruinsHeight = WorldGen.genRand.Next(20, 30);
			
			int ruinsX = 0;
			
			int dir = 1;
			
			int center = (int)(uDesert.X+uDesert.Width/2);
			
			if(Main.maxTilesX/2 < center)
			{
				dir = 1;
			}
			else
			{
				dir = -1;
			}
			
			//------ debug code
			/*if(WorldGen.crimson)
			{
				dir = -1;
			}
			else
			{
				dir = 1;
			}*/
			//------
			
			int surface = uDesert.Y+uDesert.Height/2;
			
			if(dir == 1)
			{
				//ruinsX = uDesert.X+uDesert.Width-ruinsWidth - (int)uDesert.Width/8;
				int numX = uDesert.X+uDesert.Width-ruinsWidth - (int)uDesert.Width/8;
				while(numX > uDesert.X+uDesert.Width/2+30)
				{
					int numY2 = 0;
					while(numY2 < surface)
					{
						if(Main.tile[numX, numY2].active() && Main.tile[numX, numY2].type == TileID.Sand)
						{
							break;
						}
						numY2++;
					}
					if(!Main.tile[numX, numY2-1].active())
					{
						break;
					}
					numX--;
				}
				ruinsX = numX;
			}
			else
			{
				//ruinsX = uDesert.X + (int)uDesert.Width/8;
				int numX = uDesert.X+ruinsWidth + (int)uDesert.Width/8;
				while(numX < uDesert.X+uDesert.Width/2-30)
				{
					int numY2 = 0;
					while(numY2 < surface)
					{
						if(Main.tile[numX, numY2].active() && Main.tile[numX, numY2].type == TileID.Sand)
						{
							break;
						}
						numY2++;
					}
					if(!Main.tile[numX, numY2-1].active())
					{
						break;
					}
					numX++;
				}
				ruinsX = numX-ruinsWidth;
			}
			
			progress.Message = "Chozo Ruins...Determining Y Position";
			
			int ruinsY = 0;
			while ((double)ruinsY < surface)
			{
				if(Main.tile[ruinsX, ruinsY].active() && (Main.tile[ruinsX, ruinsY].type == TileID.Sand || dir == -1))
				{
					break;
				}
				ruinsY++;
			}
			int numY = 0;
			while ((double)numY < surface)
			{
				if(Main.tile[ruinsX+ruinsWidth, numY].active() && (Main.tile[ruinsX+ruinsWidth, numY].type == TileID.Sand || dir == 1))
				{
					break;
				}
				numY++;
			}
			if(numY > ruinsY)
			{
				ruinsY = numY;
			}
			
			progress.Message = "Chozo Ruins...Building Temple";
			
			ChozoRuins_Temple(ruinsX, ruinsY-ruinsHeight+2+WorldGen.genRand.Next(3), ruinsWidth, ruinsHeight, dir);
		}
		private static void ChozoRuins_Temple(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			Hatch(x, y+height-8);
			Hatch(x+width-4, y+height-8);
			
			for(int i = 0; i < 4; i++)
			{
				WorldGen.KillWall(x, y+height-8 + i);
				WorldGen.PlaceWall(x, y+height-8 + i, WallID.SandstoneBrick);
				WorldGen.KillWall(x+width-1, y+height-8 + i);
				WorldGen.PlaceWall(x+width-1, y+height-8 + i, WallID.SandstoneBrick);
			}
			
			for(int i = 0; i < 7; i++)
			{
				WorldGen.KillTile(x-1, y+height-15 + i);
				WorldGen.PlaceTile(x-1, y+height-15 + i, TileID.SandstoneBrick);
				WorldGen.KillTile(x+width, y+height-15 + i);
				WorldGen.PlaceTile(x+width, y+height-15 + i, TileID.SandstoneBrick);
			}
			Tile.SmoothSlope(x-1, y+height-15, false);
			Tile.SmoothSlope(x+width, y+height-15, false);
			
			for(int j = 0; j < 6; j++)
			{
				for(int i = -1-j; i < width+1+j; i++)
				{
					int xx = x+i, yy = y+height-4+j;
					//if(Main.tile[xx, yy].active() || i >= -1 && i <= width)
					//{
						if(j != 0 || i < 0 || i >= width)
						{
							DestroyChest(xx, yy);
							WorldGen.KillTile(xx, yy);
						}
						WorldGen.PlaceTile(xx, yy, TileID.SandstoneBrick);
					//}
				}
			}
			
			int shaftWidth = 24;
			int shaftX = x+width-4-shaftWidth-WorldGen.genRand.Next(width/6);
			if(dir == 1)
			{
				shaftX = x+4+WorldGen.genRand.Next(width/6);
			}
			ChozoRuins_FirstShaft(shaftX, y+height-4, shaftWidth, WorldGen.genRand.Next(50,60), dir);
		}
		private static void ChozoRuins_FirstShaft(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			VerticalHatch(x+width/2-2, y);
			
			for(int i = -3; i < 3; i++)
			{
				WorldGen.PlaceTile(x+width/2+i, y+6, 19, false, false, -1, 17);
			}
			for(int j = 11; j < height - 5; j += 5)
			{
				for(int i = 0; i < 3; i++)
				{
					int platform = x+6 + Main.rand.Next(12);
					WorldGen.PlaceTile(platform-1, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform+1, y + j, 19, false, false, -1, 17);
				}
			}
			
			int chestRoomWidth = 20;
			int chestRoomHeight = 16;
			int chestRoomX = x+width-4;
			int chestRoomY = y+height-chestRoomHeight - WorldGen.genRand.Next(height/2);
			int doorX = chestRoomX;
			int doorY = chestRoomY+chestRoomHeight/2-2;
			
			int numX = doorX-2;
			if(dir == -1)
			{
				chestRoomX = x-chestRoomWidth+4;
				doorX = x;
				numX = doorX+4;
			}
			int numY = doorY+4;
			for(int i = 0; i < 2; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, TileID.SandstoneBrick);
				}
			}
			
			ChozoRuins_ChestRoom(chestRoomX,chestRoomY,chestRoomWidth,chestRoomHeight, ItemID.FlyingCarpet);
			Hatch(doorX, doorY);
			
			int morphHallWidth = WorldGen.genRand.Next(40,50);
			int morphHallHeight = 22;
			int morphHallX = x-morphHallWidth+4;
			int morphHallY = y+WorldGen.genRand.Next(16,24);
			doorX = x;
			doorY = morphHallY+morphHallHeight-10;
			numX = doorX+4;
			if(dir == -1)
			{
				morphHallX = x+width-4;
				doorX = morphHallX;
				numX = doorX-2;
			}
			numY = doorY+4;
			for(int i = 0; i < 2; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, TileID.SandstoneBrick);
				}
			}
			
			ChozoRuins_MorphHall(morphHallX,morphHallY,morphHallWidth,morphHallHeight,dir);
			Hatch(doorX, doorY);
			
			int hallWidth = WorldGen.genRand.Next(50,60);
			int hallHeight = 16;
			int hallX = x+width-hallWidth;
			int hallY = y+height-4;
			if(dir == -1)
			{
				hallX = x;
			}
			
			ChozoRuins_Hall(hallX,hallY,hallWidth,hallHeight,dir);
			VerticalHatch(x+width/2-2, y+height-4);
		}
		private static void ChozoRuins_MorphHall(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			Mod mod = MetroidMod.Instance;
			
			for(int j = 0; j < 9; j++)
			{
				
				int k = 0;
				if(j >= 3)
				{
					k = 1;
				}
				if(j >= 6)
				{
					k = 2;
				}
				if(dir == 1)
				{
					for(int i = 0; i < 4+k; i++)
					{
						WorldGen.KillTile(x+14 + i, y+height-14 + j);
						WorldGen.PlaceTile(x+14 + i, y+height-14 + j, TileID.SandstoneBrick);
					}
				}
				else
				{
					for(int i = -k; i < 4; i++)
					{
						WorldGen.KillTile(x+width-18 + i, y+height-14 + j);
						WorldGen.PlaceTile(x+width-18 + i, y+height-14 + j, TileID.SandstoneBrick);
					}
				}
				
				if(j < 5)
				{
					int numX = x+4;
					if(dir == -1)
					{
						numX = x+width-9;
					}
					WorldGen.KillTile(numX + j, y+height-5);
					WorldGen.PlaceTile(numX + j, y+height-5, TileID.SandstoneBrick);
					WorldGen.KillTile(numX-dir + j, y+height-6);
					WorldGen.PlaceTile(numX-dir + j, y+height-6, TileID.SandstoneBrick);
				}
			}
			
			int statueX = x+5;
			int statueX2 = statueX+1;
			if(dir == -1)
			{
				statueX = x+width-5;
				statueX2 = statueX-2;
			}
			int statueY = y+height-7;
			WorldGen.PlaceObject(statueX, statueY, mod.TileType("ChozoStatueNatural"), false, 0, 0, -1, -dir);
			WorldGen.PlaceObject(statueX2, statueY, mod.TileType("ChozoStatueArmNatural"), false, 0, 0, -1, -dir);
			
			WorldGen.PlaceObject(statueX2, statueY-2, mod.TileType("MorphBallTile"));
		}
		private static void ChozoRuins_Hall(int x, int y, int width, int height, int dir)
		{
			Mod mod = MetroidMod.Instance;
			
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			for(int i = 0; i < 20; i++)
			{
				for(int j = 0; j < 5; j++)
				{
					if(j < 2)
					{
						WorldGen.PlaceTile(x+width/2-10+i,y+height-6+j, TileID.SandstoneBrick);
					}
					if(i > 1 && i < 18)
					{
						WorldGen.PlaceTile(x+width/2-10+i,y+4+j, TileID.SandstoneBrick);
					}
				}
			}
			for(int i = 0; i < 10; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					if(i <= 0 || j <= 0 || i >= 9 || j >= 3)
					{
						WorldGen.KillTile(x+width/2-5+i, y+4+j);
					}
					if(i < 2 && j <= 0)
					{
						WorldGen.KillTile(x+width/2-1+i, y+8);
					}
				}
			}
			WorldGen.PlaceObject(x+width/2, y+4, mod.TileType("MissileExpansionTile"));
			
			int shaftWidth = 24;
			int shaftHeight = WorldGen.genRand.Next(70,80);
			int shaftX = x - shaftWidth + 4;
			int shaftY = y;
			int doorX = shaftX+shaftWidth-4;
			int doorY = y + height/2-2;
			int numX = doorX-2;
			if(dir == -1)
			{
				shaftX = x + width - 4;
				doorX = shaftX;
				numX = doorX+4;
			}
			int numY = doorY+4;
			ChozoRuins_SecondShaft(shaftX,shaftY,shaftWidth,shaftHeight,dir);
			Hatch(doorX, doorY);
			for(int i = 0; i < 2; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, TileID.SandstoneBrick);
				}
			}
		}
		private static void ChozoRuins_SecondShaft(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			for(int j = 11; j < height - 5; j += 5)
			{
				for(int i = 0; i < 3; i++)
				{
					int platform = x+6 + Main.rand.Next(12);
					WorldGen.PlaceTile(platform-1, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform+1, y + j, 19, false, false, -1, 17);
				}
			}
			
			int chestRoomWidth = 20;
			int chestRoomHeight = 16;
			int chestRoomX = x-chestRoomWidth+4;
			int chestRoomY = y + WorldGen.genRand.Next(height/3);
			int doorX = x;
			int doorY = chestRoomY+chestRoomHeight/2-2;
			
			int numX = doorX+4;
			if(dir == -1)
			{
				chestRoomX = x+width-4;
				doorX = chestRoomX;
				numX = doorX-2;
			}
			int numY = doorY+4;
			for(int i = 0; i < 2; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, TileID.SandstoneBrick);
				}
			}
			ChozoRuins_ChestRoom(chestRoomX,chestRoomY,chestRoomWidth,chestRoomHeight, ItemID.SandstorminaBottle);
			Hatch(doorX,doorY);
			
			int saveRoomWidth = 20;
			int saveRoomHeight = 16;
			int saveRoomX = x+width-4;
			int saveRoomY = y+height-saveRoomHeight;
			doorX = saveRoomX;
			doorY = saveRoomY+saveRoomHeight/2-2;
			numX = doorX-2;
			if(dir == -1)
			{
				saveRoomX = x-saveRoomWidth+4;
				doorX = x;
				numX = doorX+4;
			}
			numY = doorY+4;
			for(int i = 0; i < 2; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, TileID.SandstoneBrick);
				}
			}
			ChozoRuins_SaveRoom(saveRoomX,saveRoomY);
			Hatch(doorX,doorY);
			
			int bombRoomWidth = 36;
			int bombRoomHeight = 16;
			int bombRoomX = x-bombRoomWidth+4;
			int bombRoomY = y+height-bombRoomHeight;
			doorX = x;
			doorY = bombRoomY+bombRoomHeight-10;
			if(dir == -1)
			{
				bombRoomX = x+width-4;
				doorX = bombRoomX;
			}
			ChozoRuins_BombRoom(bombRoomX,bombRoomY,dir);
			Hatch(doorX,doorY);
			
			int bossRoomWidth = 80;
			int bossRoomHeight = 40;
			int bossRoomX = saveRoomX+saveRoomWidth-4;
			int bossRoomY = y+height-bossRoomHeight;
			doorX = bossRoomX;
			doorY = bossRoomY+bossRoomHeight-10;
			if(dir == -1)
			{
				bossRoomX = saveRoomX-bossRoomWidth+4;
				doorX = saveRoomX;
			}
			ChozoRuins_BossRoom(bossRoomX,bossRoomY,bossRoomWidth,bossRoomHeight,dir);
			Hatch(doorX,doorY);
		}
		private static void ChozoRuins_BombRoom(int x, int y, int dir)
		{
			Mod mod = MetroidMod.Instance;
			
			int width = 36;
			int height = 16;
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < height-9; j++)
				{
					WorldGen.PlaceTile(x+width/2-2+i, y+4+j, TileID.SandstoneBrick);
				}
				int xx = x+width/2+2+i;
				if(dir == -1)
				{
					xx = x+width/2-6+i;
				}
				WorldGen.PlaceTile(xx, y+height-6, TileID.SandstoneBrick);
			}
			
			int statueX = x+width/2+3;
			int statueX2 = statueX+1;
			if(dir == -1)
			{
				statueX = x+width/2-3;
				statueX2 = statueX-2;
			}
			int statueY = y+height-7;
			WorldGen.PlaceObject(statueX, statueY, mod.TileType("ChozoStatueNatural"), false, 0, 0, -1, -dir);
			WorldGen.PlaceObject(statueX2, statueY, mod.TileType("ChozoStatueArmNatural"), false, 0, 0, -1, -dir);
			
			WorldGen.PlaceObject(statueX2, statueY-2, mod.TileType("BombTile"));
			
			for(int i = 0; i < 5; i++)
			{
				int numX = x+4+i;
				if(dir == -1)
				{
					numX = x+width-9+i;
				}
				WorldGen.PlaceTile(numX, y+height-5, TileID.SandstoneBrick);
				WorldGen.PlaceTile(numX-dir, y+height-6, TileID.SandstoneBrick);
			}
			statueX = x+5;
			statueX2 = statueX+1;
			if(dir == -1)
			{
				statueX = x+width-5;
				statueX2 = statueX-2;
			}
			WorldGen.PlaceObject(statueX, statueY, mod.TileType("ChozoStatueNatural"), false, 0, 0, -1, -dir);
			WorldGen.PlaceObject(statueX2, statueY, mod.TileType("ChozoStatueArmNatural"), false, 0, 0, -1, -dir);
			
			WorldGen.PlaceObject(statueX2, statueY-2, mod.TileType("PowerGripTile"));
		}
		private static void ChozoRuins_BossRoom(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			int stepsX = x+4;
			if(dir == -1)
			{
				stepsX = x+width-5;
			}
			WorldGen.PlaceTile(stepsX, y+height-6, TileID.SandstoneBrick);
			WorldGen.PlaceTile(stepsX, y+height-5, TileID.SandstoneBrick);
			WorldGen.PlaceTile(stepsX+dir, y+height-5, TileID.SandstoneBrick);
			
			//NPC.NewNPC(8 + (x + width - 6) * 16, (y + height - 4) * 16, mod.NPCType("TorizoIdle"));
			TorizoRoomLocation.X = x;
			TorizoRoomLocation.Y = y;
		}
		
		private static void ChozoRuins_SaveRoom(int x, int y)
		{
			Mod mod = MetroidMod.Instance;
			
			int width = 20;
			int height = 16;
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
			for(int i = 0; i < width; i++)
			{
				for(int j = 0; j < 2; j++)
				{
					WorldGen.PlaceTile(x+i, y+height-6+j, TileID.SandstoneBrick);
					WorldGen.PlaceTile(x+i, y+4+j, TileID.SandstoneBrick);
				}
			}
			
			int numX = x+width/2-2;
			WorldGen.KillTile(numX, y+height-6);
			WorldGen.KillTile(numX+1, y+height-6);
			WorldGen.KillTile(numX+2, y+height-6);
			WorldGen.KillTile(numX+3, y+height-6);
			Tile.SmoothSlope(numX-1, y+height-6, false);
			Tile.SmoothSlope(numX+4, y+height-6, false);
			
			WorldGen.PlaceObject(numX+2, y+height-6, mod.TileType("SaveStation"), false, 0, 0, -1, 1);
			
			WorldGen.KillTile(numX, y+5);
			WorldGen.KillTile(numX+1, y+5);
			WorldGen.KillTile(numX+2, y+5);
			WorldGen.KillTile(numX+3, y+5);
			Tile.SmoothSlope(numX-1, y+5, false);
			Tile.SmoothSlope(numX+4, y+5, false);
			
			WorldGen.PlaceTile(x+6, y+6, TileID.SandstoneBrick);
			WorldGen.PlaceTile(x+width-7, y+6, TileID.SandstoneBrick);
			WorldGen.PlaceObject(x+6, y+7, 10, false, 29, 0, -1, 1);
			WorldGen.PlaceObject(x+width-7, y+7, 10, false, 29, 0, -1, 1);
		}
		
		private static void ChozoRuins_ChestRoom(int x, int y, int width, int height, int itemType)
		{
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick,1);
			
			for(int j = 0; j < 2; j++)
			{
				for(int i = -j; i < 4+j; i++)
				{
					int numX = x+width/2-2;
					WorldGen.KillTile(numX + i, y+height-6 + j);
					WorldGen.PlaceTile(numX + i, y+height-6 + j, TileID.SandstoneBrick);
				}
			}
			Mod mod = MetroidMod.Instance;
			int xx = x+width/2;
			int yy = y+height-7;
			WorldGen.AddBuriedChest(xx, yy, itemType, false, 1);
			for (int l = xx-1; l < xx+1; l++)
			{
				for (int m = yy-1; m < yy+1; m++)
				{
					if (Main.tile[l,m] == null)
						Main.tile[l,m] = new Tile();
					Tile tile = Main.tile[l,m];
					tile.active(true);
					tile.type = (ushort)mod.TileType("ChozoChest");
					tile.frameX = (short)((tile.frameX / 18 % 2) * 18);
					tile.frameY = (short)((tile.frameY / 18 % 2) * 18);
				}
			}
		}
		
		private static void BasicStructure(int x, int y, int width, int height, int thickness, int tileType, int wallType, int ruinedWallType = 0, int ruinedWallRand = 6)
		{
			int thick = thickness;
			if(thick < 1)
			{
				thick = 1;
			}
			for(int i = 0; i < width; i++)
			{
				for(int j = 0; j < height; j++)
				{
					int wallType2 = wallType;
					int rand = WorldGen.genRand.Next(ruinedWallRand);
					if(rand == 0 && ruinedWallType >= 1)
					{
						wallType2 = -1;
					}
					if(rand == 1 && ruinedWallType >= 2)
					{
						wallType2 = 0;
					}
					if(i > 0 && i < width-1 && j > 0 && j < height-1 && wallType2 >= 0)
					{
						WorldGen.KillWall(x + i, y + j);
						WorldGen.PlaceWall(x + i, y + j, wallType2);
					}
					DestroyChest(x + i, y + j);
					WorldGen.KillTile(x + i, y + j);
					
					if(i < thick || j < thick || i >= width-thick || j >= height-thick)
					{
						WorldGen.PlaceTile(x + i, y + j, TileID.SandstoneBrick);
					}
				}
			}
		}
		private static void Hatch(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int x = i; x < i + 4; x++)
			{
				for (int y = j; y < j + 4; y++)
				{
					DestroyChest(x, y);
					WorldGen.KillTile(x, y);
				}
			}
			WorldGen.PlaceObject(i + 1, j + 2, mod.TileType("BlueHatch"), false, 0, 0, -1, 1);
		}
		private static void VerticalHatch(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int x = i; x < i + 4; x++)
			{
				for (int y = j; y < j + 4; y++)
				{
					DestroyChest(x, y);
					WorldGen.KillTile(x, y);
				}
			}
			WorldGen.PlaceObject(i + 1, j + 2, mod.TileType("BlueHatchVertical"), false, 0, 0, -1, 1);
		}
		private static void DestroyChest(int x, int y)
		{
			if(!Chest.DestroyChest(x, y))
			{
				int id = Chest.FindChest(x, y);
				Chest.DestroyChestDirect(x, y, id);
			}
		}

		int spawnCounter = 0;
		int spawnCounter2 = 0;
		public override void PostUpdate()
		{
			if(Main.hardMode && NPC.downedPlantBoss && !spawnedPhazonMeteor)
			{
				DropPhazonMeteor();
			}
			
			if(!bossesDown.HasFlag(MetroidBossDown.downedTorizo) && !NPC.AnyNPCs(mod.NPCType("Torizo")) && !NPC.AnyNPCs(mod.NPCType("IdleTorizo")) && TorizoRoomLocation.X > 0 && TorizoRoomLocation.Y > 0)
			{
				Rectangle room = TorizoRoomLocation;
				if(spawnCounter <= 0)
				{
					Vector2 pos = new Vector2(room.X+8,room.Y+room.Height-4);
					if(room.X > Main.maxTilesX/2)
					{
						pos.X = (room.X+room.Width-8);
					}
					pos *= 16f;
					
					NPC.NewNPC((int)pos.X,(int)pos.Y,mod.NPCType("IdleTorizo"));
				}
				else
				{
					spawnCounter--;
				}
			}
			else
			{
				spawnCounter = 300;
			}
			
			if(NPC.downedGolemBoss && bossesDown.HasFlag(MetroidBossDown.downedTorizo) &&
				!bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo) && !NPC.AnyNPCs(mod.NPCType("GoldenTorizo")) && !NPC.AnyNPCs(mod.NPCType("IdleGoldenTorizo")) && TorizoRoomLocation.X > 0 && TorizoRoomLocation.Y > 0)
			{
				Rectangle room = TorizoRoomLocation;
				if(spawnCounter2 <= 0)
				{
					Vector2 pos = new Vector2(room.X+8,room.Y+room.Height-4);
					if(room.X > Main.maxTilesX/2)
					{
						pos.X = (room.X+room.Width-8);
					}
					pos *= 16f;
					
					NPC.NewNPC((int)pos.X,(int)pos.Y,mod.NPCType("IdleGoldenTorizo"));
				}
				else
				{
					spawnCounter2--;
				}
			}
			else
			{
				spawnCounter2 = 300;
			}
		}
    }
}
