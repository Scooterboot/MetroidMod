using System.IO;
using System.Collections.Generic;
using System.Linq;
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
		public override void Initialize()
		{
			downedTorizo = false;
			downedSerris = false;
			downedKraid = false;
			downedPhantoon = false;
		}

		public override TagCompound Save()
		{
			var downed = new List<string>();
			if (downedTorizo) downed.Add("Torizo");
			if (downedSerris) downed.Add("Serris");
			if (downedKraid) downed.Add("Kraid");
			if (downedPhantoon) downed.Add("Phantoon");

			return new TagCompound {
				{"downed", downed}
			};
		}

		public override void Load(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedTorizo = downed.Contains("Torizo");
			downedSerris = downed.Contains("Serris");
			downedKraid = downed.Contains("Kraid");
			downedPhantoon = downed.Contains("Phantoon");
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
			writer.Write(flags);

		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedTorizo = flags[0];
			downedSerris = flags[1];
			downedKraid = flags[2];
			downedPhantoon = flags[3];
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
						int rand = Main.rand.Next(12);
						ushort item = (ushort)mod.TileType("MorphBallTile");
						if (rand == 1 || rand == 2)
						{
							item = (ushort)mod.TileType("ChargeBeamTile");
						}
						else if (rand == 3 || rand == 4)
						{
							item = (ushort)mod.TileType("IceBeamTile");
						}
						else if (rand == 5)
						{
							item = (ushort)mod.TileType("WaveBeamTile");
						}
						else if (rand == 6)
						{
							item = (ushort)mod.TileType("SpazerTile");
						}
						else if (rand == 7)
						{
							item = (ushort)mod.TileType("XRayScopeTile");
						}
						else if (rand == 8 || rand == 9)
						{
							item = (ushort)mod.TileType("SpaceJumpBootsTile");
						}
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
					WorldGen.PlaceWall(ruinsX + 1 + i, ruinsY + 1 + j, 5);
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
			Hatch(ruinsX - 60, ruinsY + 26);
			Hatch(ruinsX - 84, ruinsY + 26);
			VerticalHatch(ruinsX - 73, ruinsY - 26);
			WorldGen.PlaceTile(ruinsX - 71, ruinsY - 19, 19, false, false, -1, 9);
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
		public static void DeathHall(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int wx = i + 1; wx < i + 49; wx++)
			{
				for (int wy = j + 1; wy < j + 15; wy++)
				{
					WorldGen.KillWall(wx, wy);
					WorldGen.KillTile(wx, wy);
					WorldGen.PlaceWall(wx, wy, 5);
				}
			}
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
			}
			for (int y = j + 1; y < j + 15; y++)
			{
				WorldGen.KillTile(i, y);
				WorldGen.KillTile(i + 49, y);
				WorldGen.PlaceTile(i, y, TileID.GrayBrick);
				WorldGen.PlaceTile(i + 49, y, TileID.GrayBrick);
			}
			WorldGen.PlaceTile(i + 1, j + 6, TileID.GrayBrick);
			WorldGen.PlaceTile(i + 48, j + 6, TileID.GrayBrick);
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
						WorldGen.PlaceWall(wx, wy, 5);
					}
				if (wy % 5 == 0)
				{
					int platform = 2 + Main.rand.Next(19);
					WorldGen.PlaceTile(i + platform - 1, wy, 19, false, false, -1, 9);
					WorldGen.PlaceTile(i + platform, wy, 19, false, false, -1, 9);
					WorldGen.PlaceTile(i + platform + 1, wy, 19, false, false, -1, 9);
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
    }
}
