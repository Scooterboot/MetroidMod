using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod
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
		downedOmegaPirate = 1<<5
	}

    public class MWorld : ModWorld
    {
		public static MetroidBossDown bossesDown;
		public static bool spawnedPhazonMeteor = false;
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
			spawnedPhazonMeteor = tag.Get<bool>("spawnedPhazonMeteor");
			
			if(tag.HasTag("TorizoRoomLocation.X") && tag.HasTag("TorizoRoomLocation.Y"))
			{ // this if statement is temporary. once a world has been loaded and saved, all that is needed are the two lines of code below.
				TorizoRoomLocation.X = tag.GetAsInt("TorizoRoomLocation.X");
				TorizoRoomLocation.Y = tag.GetAsInt("TorizoRoomLocation.Y");
			}
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
						ushort item = StatueItem();
						while (!flag)
						{
							if (AddChozoStatue(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)WorldGen.worldSurface, Main.maxTilesY - 100), item))
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
					for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); i++)
					{
						float num2 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05));
						bool flag = false;
						int num3 = 0;
						while (!flag)
						{
							if (AddExpansion(WorldGen.genRand.Next(1, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.worldSurface, Main.maxTilesY - 100)))
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
			BasicStructure(x,y,width,height,4,TileID.SandstoneBrick,WallID.SandstoneBrick);
			
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
		
		private static void BasicStructure(int x, int y, int width, int height, int thickness, int tileType, int wallType)
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
					if(i > 0 && i < width-1 && j > 0 && j < height-1)
					{
						WorldGen.KillWall(x + i, y + j);
						WorldGen.PlaceWall(x + i, y + j, wallType);
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
		
		/*public override void PostWorldGen()
		{
			int ruinsX = Main.spawnTileX + 150;
			int ruinsY = Main.spawnTileY + 470;
			for (int i = 0; i < 69; i++)
			{
				for (int j = 0; j < 29; j++)
				{
					WorldGen.KillWall(ruinsX + 1 + i, ruinsY + 1 + j);
					WorldGen.KillTile(ruinsX + 1 + i, ruinsY + 1 + j);
					if (Main.rand.Next(3) < 2)
					{
						WorldGen.PlaceWall(ruinsX + 1 + i, ruinsY + 1 + j, 5);
					}
				}
				WorldGen.KillTile(ruinsX + i, ruinsY);
				WorldGen.KillTile(ruinsX + i, ruinsY + 30);
				WorldGen.PlaceTile(ruinsX + i, ruinsY, TileID.GrayBrick);
				WorldGen.PlaceTile(ruinsX + i, ruinsY + 30, TileID.GrayBrick);	
			}				
			for (int k = 0; k < 30; k++)
			{
				WorldGen.KillTile(ruinsX, ruinsY + k);
				WorldGen.KillTile(ruinsX + 69, ruinsY + k);
				WorldGen.PlaceTile(ruinsX, ruinsY + k, TileID.GrayBrick);
				WorldGen.PlaceTile(ruinsX + 69, ruinsY + k, TileID.GrayBrick);
			}
			NPC.NewNPC(8 + (Main.spawnTileX + 218) * 16, (Main.spawnTileY + 500) * 16, mod.NPCType("TorizoIdle"));
			DeathHall(ruinsX - 58, ruinsY + 24);
			Shaft(ruinsX - 82, ruinsY - 24);
			VerticalHatch(ruinsX - 73, ruinsY - 26);
			WorldGen.PlaceTile(ruinsX - 71, ruinsY - 19, 19, false, false, -1, 9);
			Hatch(ruinsX - 60, ruinsY + 26);
			Hall(ruinsX - 123, ruinsY + 21);
			Hatch(ruinsX - 84, ruinsY + 26);
			Shaft(ruinsX - 147, ruinsY + 26);
			Hatch(ruinsX - 125, ruinsY + 26);
			WorldGen.PlaceTile(ruinsX - 126, ruinsY + 30, 19, false, false, -1, 9);
			DeathHall(ruinsX - 123, ruinsY + 74);
			Hatch(ruinsX - 125, ruinsY + 76);
			ChozoRoom(ruinsX - 72, ruinsY + 71);
			Hatch(ruinsX - 74, ruinsY + 76);
			SaveRoom(ruinsX - 9, ruinsY + 24);		
		}
		public static void Hatch(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int x = i; x < i + 3; x++)
			{
				for (int y = j; y < j + 5; y++)
				{
					WorldGen.KillTile(x, y);
				}
				WorldGen.PlaceTile(x, j, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 4, TileID.GrayBrick);
			}
			WorldGen.PlaceObject(i + 1, j + 3, mod.TileType("BlueHatch"), false, 0, 0, -1, 1);
		}
		public static void VerticalHatch(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int x = i; x < i + 5; x++)
			{
				for (int y = j; y < j + 3; y++)
				{
					WorldGen.KillTile(x, y);	
					WorldGen.PlaceTile(i, y, TileID.GrayBrick);
					WorldGen.PlaceTile(i + 4, y, TileID.GrayBrick);
				}
			}
			WorldGen.PlaceObject(i + 2, j + 2, mod.TileType("BlueHatchVertical"), false, 0, 0, -1, 1);
		}
		public static void SaveRoom(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int wx = i + 1; wx < i + 9; wx++)
			{
				for (int wy = j + 1; wy < j + 6; wy++)
				{
					WorldGen.KillWall(wx, wy);
					WorldGen.KillTile(wx, wy);
					WorldGen.PlaceWall(wx, wy, 5);
				}
			}
			for (int x = i; x < i + 10; x++)
			{
				WorldGen.KillTile(x, j);
				WorldGen.KillTile(x, j + 6);
				WorldGen.PlaceTile(x, j, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 6, TileID.GrayBrick);
			}
			for (int y = j + 1; y < j + 6; y++)
			{
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 9, y);
			}
			WorldGen.PlaceTile(i, j + 1, TileID.GrayBrick);
			WorldGen.PlaceTile(i + 9, j+ 1, TileID.GrayBrick);
			WorldGen.PlaceTile(i, j + 2, TileID.GrayBrick);
			WorldGen.PlaceTile(i + 9, j+ 2, TileID.GrayBrick);
			WorldGen.PlaceObject(i, j + 5, 10, false, 10, 0, -1, 1);
			WorldGen.PlaceObject(i + 9, j + 5, 10, false, 10, 0, -1, 1);
			WorldGen.PlaceObject(i + 5, j + 5, mod.TileType("SaveStation"), false, 0, 0, -1, 1);	
		}
		public static void ChozoRoom(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int wx = i + 1; wx < i + 29; wx++)
			{
				for (int wy = j + 1; wy < j + 10; wy++)
				{
					WorldGen.KillWall(wx, wy);
					WorldGen.KillTile(wx, wy);
					WorldGen.PlaceWall(wx, wy, 5);
				}
			}
			for (int x = i; x < i + 30; x++)
			{
				WorldGen.KillTile(x, j);
				WorldGen.KillTile(x, j + 10);
				WorldGen.PlaceTile(x, j, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 10, TileID.GrayBrick);
			}
			for (int y = j + 1; y < j + 10; y++)
			{
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 15, y);
				WorldGen.PlaceTile(i, y, TileID.GrayBrick);
				WorldGen.PlaceTile(i + 15, y, TileID.GrayBrick);
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 29, y);
				WorldGen.PlaceTile(i, y, TileID.GrayBrick);
				WorldGen.PlaceTile(i + 29, y, TileID.GrayBrick);
			}
			for (int h = i + 16; h < i + 29; h++)
			{
				WorldGen.KillTile(h, j + 2);
				WorldGen.PlaceTile(h, j + 2, TileID.GrayBrick);
				WorldGen.KillTile(h, j + 4);
				WorldGen.PlaceTile(h, j + 4, TileID.GrayBrick);
				WorldGen.KillTile(h, j + 6);
				WorldGen.PlaceTile(h, j + 6, TileID.GrayBrick);
				WorldGen.KillTile(h, j + 8);
				WorldGen.PlaceTile(h, j + 8, TileID.GrayBrick);
			}
			WorldGen.KillTile(i + 16, j + 2);
			WorldGen.KillTile(i + 25, j + 4);
			WorldGen.KillTile(i + 18, j + 6);
			WorldGen.KillTile(i + 27, j + 8);
			WorldGen.PlaceTile(i + 27, j + 1, mod.TileType("MissileExpansionTile"));
			for (int c = i + 8; c < i + 15; c++)
			{
				WorldGen.KillTile(c, j + 8);
				WorldGen.PlaceTile(c, j + 8, TileID.GrayBrick);
			}
			WorldGen.PlaceObject(i + 14, j + 7, mod.TileType("ChozoStatueNatural"), false, 0, 0, -1, 1);	
			WorldGen.PlaceObject(i + 12, j + 7, mod.TileType("ChozoStatueArmNatural"), false, 0, 0, -1, 1);
			ushort item = StatueItem();
			WorldGen.PlaceTile(i + 12, j + 5, item);
			WorldGen.KillTile(i + 15, j + 9);
		}
		public static void DeathHall(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int wx = i + 1; wx < i + 49; wx++)
			{
				for (int wy = j + 1; wy < j + 15; wy++)
				{
					WorldGen.KillWall(wx, wy);
					WorldGen.KillTile(wx, wy);
					if (Main.rand.Next(3) < 2)
					{
						WorldGen.PlaceWall(wx, wy, 5);
					}
				}
			}
			WorldGen.PlaceTile(i + 1, j + 6, TileID.GrayBrick);
			WorldGen.PlaceTile(i + 48, j + 6, TileID.GrayBrick);
			for (int x = i; x < i + 50; x++)
			{
				WorldGen.KillTile(x, j);
				WorldGen.KillTile(x, j + 15);
				WorldGen.PlaceTile(x, j, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 15, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 14, TileID.Spikes);
				if (x % 2 == 0)
				{
					WorldGen.PlaceTile(x, j + 13, TileID.Spikes);
				}
				if ((x - i) % 5 == 0)
				{
					WorldGen.PlaceTile(x+2, j + 6, 19, false, false, -1, 9);
					if (Main.rand.Next(2) == 0)
					{
						WorldGen.PlaceTile(x+1+(2*Main.rand.Next(2)), j + 6, 19, false, false, -1, 9);
					}
				}
			}
			for (int y = j + 1; y < j + 15; y++)
			{
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 49, y);
				WorldGen.PlaceTile(i, y, TileID.GrayBrick);
				WorldGen.PlaceTile(i + 49, y, TileID.GrayBrick);
			}
		}
		public static void Hall(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int wx = i + 1; wx < i + 39; wx++)
			{
				for (int wy = j + 1; wy < j + 9; wy++)
				{
					WorldGen.KillWall(wx, wy);
					WorldGen.KillTile(wx, wy);
					if (Main.rand.Next(3) < 2)
					{
						WorldGen.PlaceWall(wx, wy, 5);
					}
				}
			}
			for (int x = i; x < i + 40; x++)
			{
				WorldGen.KillTile(x, j);
				WorldGen.KillTile(x, j + 9);
				WorldGen.PlaceTile(x, j, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 9, TileID.GrayBrick);
			}
			for (int y = j + 1; y < j + 9; y++)
			{
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 39, y);
				WorldGen.PlaceTile(i, y, TileID.GrayBrick);
				WorldGen.PlaceTile(i + 39, y, TileID.GrayBrick);
			}
		}
		public static void Shaft(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int wy = j + 1; wy < j + 54; wy++)
				{
					for (int wx = i + 1; wx < i + 22; wx++)
					{		
						WorldGen.KillWall(wx, wy);
						WorldGen.KillTile(wx, wy);
						if (Main.rand.Next(3) < 2)
						{
							WorldGen.PlaceWall(wx, wy, 5);
						}
					}
				if (((wy-1) - j) % 5 == 0)
				{
					int platform = 2 + Main.rand.Next(19);
					WorldGen.PlaceTile(i + platform - 1, wy - 1, 19, false, false, -1, 9);
					WorldGen.PlaceTile(i + platform, wy - 1, 19, false, false, -1, 9);
					WorldGen.PlaceTile(i + platform + 1, wy - 1, 19, false, false, -1, 9);
				}
			}
			for (int x = i; x < i + 23; x++)
			{
				WorldGen.KillTile(x, j);
				WorldGen.KillTile(x, j + 54);
				WorldGen.PlaceTile(x, j, TileID.GrayBrick);
				WorldGen.PlaceTile(x, j + 54, TileID.GrayBrick);
			}
			for (int y = j + 1; y < j + 55; y++)
			{
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 22, y);
				WorldGen.PlaceTile(i, y, TileID.GrayBrick);
				WorldGen.PlaceTile(i + 22, y, TileID.GrayBrick);
			}
		}*/


		public static bool AddExpansion(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			int k = j;
			while (k < Main.maxTilesY)
			{
				if (Main.tile[i, k].active() && Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tile[i, k - 1].active())
				{
					int num = k - 1;
					if (Main.tile[i, num - 1].lava())
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
		public static ushort StatueItem()
		{
			Mod mod = MetroidMod.Instance;
			int rand = Main.rand.Next(15);
			ushort item = (ushort)mod.TileType("MorphBallTile");
			if (rand == 1 || rand == 2)
			{
				item = (ushort)mod.TileType("ChargeBeamTile");
			}
			else if (rand == 3)
			{
				item = (ushort)mod.TileType("IceBeamTile");
			}
			else if (rand == 4)
			{
				item = (ushort)mod.TileType("WaveBeamTile");
			}
			else if (rand == 5)
			{
				item = (ushort)mod.TileType("SpazerTile");
			}
			else if (rand == 6)
			{
				item = (ushort)mod.TileType("XRayScopeTile");
			}
			else if (rand == 7 || rand == 8)
			{
				item = (ushort)mod.TileType("SpaceJumpBootsTile");
			}
			else if (rand == 9 || rand == 10)
			{
				item = (ushort)mod.TileType("BombTile");
			}
			else if (rand == 11)
			{
				item = (ushort)mod.TileType("SpiderBallTile");
			}
			else if (rand == 12)
			{
				item = (ushort)mod.TileType("BoostBallTile");
			}
			return item;
		}
		public static bool AddChozoStatue(int i, int j, ushort item)
		{
			Mod mod = MetroidMod.Instance;
			int k = j;
			while (k < Main.maxTilesY)
			{
				if (Main.tile[i, k].active() && Main.tileSolid[(int)Main.tile[i, j].type] && Main.tile[i + 1, k].active() && Main.tileSolid[(int)Main.tile[i + 1, j].type] && Main.tile[i + 2, k].active() && Main.tileSolid[(int)Main.tile[i + 2, j].type] && !Main.tile[i, k - 1].active() && !Main.tile[i, k - 2].active() && !Main.tile[i, k - 3].active() && !Main.tile[i + 1, k - 1].active() && !Main.tile[i + 1, k - 2].active() && !Main.tile[i + 1, k - 3].active() && !Main.tile[i + 2, k - 1].active() && !Main.tile[i + 2, k - 2].active() && !Main.tile[i + 2, k - 3].active())
				{
					int num = k - 1;
					if (Main.tile[i, num - 1].lava() || Main.tile[i + 1, num - 1].lava() || Main.tile[i + 1, num - 1].lava())
					{
						return false;
					}
					for (int wx = i - 1; wx < i + 4; wx++)
					{
						for (int wy = k - 4; wy < k; wy++)
						{
							WorldGen.KillWall(wx, wy);
							WorldGen.KillTile(wx, wy);
							WorldGen.PlaceWall(wx, wy, 5);
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
					for (int tx2 = i - 1; tx2 < i + 4; tx2++)
					{
						Main.tile[tx2, k].slope(0);	
						Main.tile[tx2, k].halfBrick(false);
						Main.tile[tx2, k].active(true);	
						Main.tile[tx2, k].type = 38;
						Main.tile[tx2, k - 6].slope(0);	
						Main.tile[tx2, k - 6].halfBrick(false);	
						Main.tile[tx2, k - 6].active(true);	
						Main.tile[tx2, k - 6].type = 38;
					}
					Main.tile[i - 2, k - 4].slope(0);	
					Main.tile[i - 2, k - 4].halfBrick(false);
					Main.tile[i - 2, k - 4].active(true);	
					Main.tile[i - 2, k - 4].type = 38;

					Main.tile[i + 4, k - 4].slope(0);	
					Main.tile[i + 4, k - 4].halfBrick(false);
					Main.tile[i + 4, k - 4].active(true);	
					Main.tile[i + 4, k - 4].type = 38;

					WorldGen.PlaceObject(i + 2, num, mod.TileType("ChozoStatueNatural"), false, 0, 0, -1, 1);	
					WorldGen.PlaceObject(i, num, mod.TileType("ChozoStatueArmNatural"), false, 0, 0, -1, 1);
					
					Main.tile[i, num - 2].active(true);
					Main.tile[i, num - 2].type = item;
					Main.tile[i, num - 2].frameX = 0;
					Main.tile[i, num - 2].frameY = 0;
					
					return true;
				}
				else
				{
					k++;
				}
			}
			return false;
		}

		int spawnCounter = 0;
		public override void PostUpdate()
		{
			/*if(Main.hardMode && !spawnedPhazonMeteor)
			{
				DropPhazonMeteor();
			}*/
			
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
		}
		public static void AddPhazon() 
		{
			Mod mod = MetroidMod.Instance;
			int lX = 200;
			int hX = Main.maxTilesX-200;
			int lY = (int)Main.worldSurface;
			int hY = Main.maxTilesY-200;
			int minSpread = 5;
			int maxSpread = 8;
			int minFrequency = 5;
			int maxFrequency = 8;
			WorldGen.OreRunner(WorldGen.genRand.Next(lX,hX), WorldGen.genRand.Next(lY,hY), (double)WorldGen.genRand.Next(minSpread,maxSpread+1), WorldGen.genRand.Next(minFrequency,maxFrequency+1), (ushort)mod.TileType("PhazonTile"));
		}
		public static void DropPhazonMeteor()
		{
			Mod mod = MetroidMod.Instance;
			bool flag = false;//true;
			int num = 0;
			if (Main.netMode == 1)
			{
				return;
			}
			/*for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					flag = false;
					break;
				}
			}*/
			int num2 = 0;
			float num3 = (float)(Main.maxTilesX / 4200);
			int num4 = (int)(400f * num3);
			for (int j = 5; j < Main.maxTilesX - 5; j++)
			{
				int num5 = 5;
				while ((double)num5 < Main.worldSurface)
				{
					if (Main.tile[j, num5].active() && Main.tile[j, num5].type == mod.TileType("PhazonTile"))
					{
						num2++;
						if (num2 > num4)
						{
							return;
						}
					}
					num5++;
				}
			}
			while (!flag)
			{
				float num6 = (float)Main.maxTilesX * 0.08f;
				int num7 = Main.rand.Next(50, Main.maxTilesX - 50);
				while ((float)num7 > (float)Main.spawnTileX - num6 && (float)num7 < (float)Main.spawnTileX + num6)
				{
					num7 = Main.rand.Next(50, Main.maxTilesX - 50);
				}
				for (int k = Main.rand.Next(100); k < Main.maxTilesY; k++)
				{
					if (Main.tile[num7, k].active() && Main.tileSolid[(int)Main.tile[num7, k].type])
					{
						flag = phazonMeteor(num7, k);
						break;
					}
				}
				num++;
				if (num >= 100)
				{
					return;
				}
			}
			spawnedPhazonMeteor = flag;
		}
		private static bool phazonMeteor(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			if (i < 50 || i > Main.maxTilesX - 50)
			{
				return false;
			}
			if (j < 50 || j > Main.maxTilesY - 50)
			{
				return false;
			}
			if (i < (Main.maxTilesX/2) - 50 ||
				i < (Main.maxTilesX/2) + 50)
			{
				return false;
			}
			int num = 25;
			Rectangle rectangle = new Rectangle((i - num) * 16, (j - num) * 16, num * 2 * 16, num * 2 * 16);
			for (int k = 0; k < 255; k++)
			{
				if (Main.player[k].active)
				{
					Rectangle value = new Rectangle((int)(Main.player[k].position.X + (float)(Main.player[k].width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(Main.player[k].position.Y + (float)(Main.player[k].height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
					if (rectangle.Intersects(value))
					{
						return false;
					}
				}
			}
			for (int l = 0; l < 200; l++)
			{
				if (Main.npc[l].active)
				{
					Rectangle value2 = new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height);
					if (rectangle.Intersects(value2))
					{
						return false;
					}
				}
			}
			for (int m = i - num; m < i + num; m++)
			{
				for (int n = j - num; n < j + num; n++)
				{
					if (Main.tile[m, n].active() && Main.tile[m, n].type == 21)
					{
						return false;
					}
				}
			}
			num = 15;
			for (int num2 = i - num; num2 < i + num; num2++)
			{
				for (int num3 = j - num; num3 < j + num; num3++)
				{
					if (num3 > j + Main.rand.Next(-2, 3) - 5 && (double)(Math.Abs(i - num2) + Math.Abs(j - num3)) < (double)num * 1.5 + (double)Main.rand.Next(-5, 5))
					{
						if (!Main.tileSolid[(int)Main.tile[num2, num3].type])
						{
							Main.tile[num2, num3].active(false);
						}
						Main.tile[num2, num3].type = (ushort)mod.TileType("PhazonTile");
					}
				}
			}
			num = 10;
			for (int num4 = i - num; num4 < i + num; num4++)
			{
				for (int num5 = j - num; num5 < j + num; num5++)
				{
					if (num5 > j + Main.rand.Next(-2, 3) - 5 && Math.Abs(i - num4) + Math.Abs(j - num5) < num + Main.rand.Next(-3, 4))
					{
						Main.tile[num4, num5].active(false);
					}
				}
			}
			num = 5;
			for (int num4 = i - num; num4 < i + num; num4++)
			{
				for (int num5 = j + 4 - num; num5 < j + 4 + num; num5++)
				{
					if (num5 > j + Main.rand.Next(-2, 3) - 5 && (double)(Math.Abs(i - num4) + Math.Abs(j - num5)) < (double)num * 1.5 + (double)Main.rand.Next(-5, 5))
					{
						if (!Main.tileSolid[(int)Main.tile[num4, num5].type])
						{
							Main.tile[num4, num5].active(false);
						}
						WorldGen.PlaceTile(num4, num5, mod.TileType("PhazonCore"), true, false, -1, 0);
						WorldGen.SquareTileFrame(num4, num5, true);
					}
				}
			}
			num = 16;
			for (int num6 = i - num; num6 < i + num; num6++)
			{
				for (int num7 = j - num; num7 < j + num; num7++)
				{
					if (Main.tile[num6, num7].type == 5 || Main.tile[num6, num7].type == 32)
					{
						WorldGen.KillTile(num6, num7, false, false, false);
					}
					WorldGen.SquareTileFrame(num6, num7, true);
					WorldGen.SquareWallFrame(num6, num7, true);
				}
			}
			num = 23;
			for (int num8 = i - num; num8 < i + num; num8++)
			{
				for (int num9 = j - num; num9 < j + num; num9++)
				{
					if (Main.tile[num8, num9].active() && Main.rand.Next(10) == 0 && (double)(Math.Abs(i - num8) + Math.Abs(j - num9)) < (double)num * 1.3)
					{
						if (Main.tile[num8, num9].type == 5 || Main.tile[num8, num9].type == 32)
						{
							WorldGen.KillTile(num8, num9, false, false, false);
						}
						Main.tile[num8, num9].type = (ushort)mod.TileType("PhazonTile");
						WorldGen.SquareTileFrame(num8, num9, true);
					}
				}
			}
			if (Main.netMode == 0)
			{
				Main.NewText("A Phazon Meteor has landed!", 50, 255, 130);
			}
			/*else if (Main.netMode == 2)
			{
				NetMessage.SendData(25, -1, -1, "A Phazon Meteor has landed!", 255, 50f, 255f, 130f, 0);
			}*/
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j, 30);
			}
			return true;
		}
    }
}
