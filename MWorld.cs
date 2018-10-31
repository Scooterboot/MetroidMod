

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
    public class MWorld : ModWorld
    {
		public static bool downedTorizo = false;
		public static bool downedSerris = false;
		public static bool downedKraid = false;
		public static bool downedPhantoon = false;
		public static bool spawnedPhazonMeteor = false;
		public static bool downedNightmare = false;
		public static bool downedOmegaPirate = false;
		public override void Initialize()
		{
			downedTorizo = false;
			downedSerris = false;
			downedKraid = false;
			downedPhantoon = false;
			spawnedPhazonMeteor = false;
			downedNightmare = false;
			downedOmegaPirate = false;
		}

		public override TagCompound Save()
		{
			var downed = new List<string>();
			if (downedTorizo) downed.Add("Torizo");
			if (downedSerris) downed.Add("Serris");
			if (downedKraid) downed.Add("Kraid");
			if (downedPhantoon) downed.Add("Phantoon");
			if (downedNightmare) downed.Add("Nightmare");
			if (downedOmegaPirate) downed.Add("OmegaPirate");

			return new TagCompound {
				{"downed", downed},
				{"spawnedPhazonMeteor", spawnedPhazonMeteor}
			};
		}

		public override void Load(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedTorizo = downed.Contains("Torizo");
			downedSerris = downed.Contains("Serris");
			downedKraid = downed.Contains("Kraid");
			downedPhantoon = downed.Contains("Phantoon");
			spawnedPhazonMeteor = tag.Get<bool>("spawnedPhazonMeteor");
			downedNightmare = downed.Contains("Nightmare");
			downedOmegaPirate = downed.Contains("OmegaPirate");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				downedTorizo = flags[0];
				downedSerris = flags[1];
				downedKraid = flags[2];
				downedPhantoon = flags[3];
				spawnedPhazonMeteor = flags[4];
				downedNightmare = flags[5];
				downedOmegaPirate = flags[6];
			}
			else
			{
				ErrorLogger.Log("MetroidMod: Unknown loadVersion: " + loadVersion);
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = downedTorizo;
			flags[1] = downedSerris;
			flags[2] = downedKraid;
			flags[3] = downedPhantoon;
			flags[4] = spawnedPhazonMeteor;
			flags[5] = downedNightmare;
			flags[6] = downedOmegaPirate;
			writer.Write(flags);

		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedTorizo = flags[0];
			downedSerris = flags[1];
			downedKraid = flags[2];
			downedPhantoon = flags[3];
			spawnedPhazonMeteor = flags[4];
			downedNightmare = flags[5];
			downedOmegaPirate = flags[6];
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
				tasks.Insert(PotsIndex - 2, new PassLegacy("Chozo Statues", delegate (GenerationProgress progress)
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
				tasks.Insert(PotsIndex - 1, new PassLegacy("Missile Expansions", delegate (GenerationProgress progress)
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
			}
		}
		public override void PostWorldGen()
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

		public override void PostUpdate()
		{
			if(Main.hardMode && !spawnedPhazonMeteor)
			{
				DropPhazonMeteor();
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
