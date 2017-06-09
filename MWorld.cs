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
		public static bool downedPhantoon = false;
		public override void Initialize()
		{
			downedTorizo = false;
			downedSerris = false;
			downedPhantoon = false;
		}

		public override TagCompound Save()
		{
			var downed = new List<string>();
			if (downedTorizo) downed.Add("Torizo");
			if (downedSerris) downed.Add("Serris");
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
				downedPhantoon = flags[2];
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
			flags[2] = downedPhantoon;
			writer.Write(flags);

		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedTorizo = flags[0];
			downedSerris = flags[1];
			downedPhantoon = flags[2];
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

					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 5E-05); k++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY), (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(4, 7), mod.TileType("ChoziteOreTile"), false, 0f, 0f, false, true);
					}
				}));
				tasks.Insert(PotsIndex + 1, new PassLegacy("Missile Expansions", delegate (GenerationProgress progress)
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
				tasks.Insert(PotsIndex + 2, new PassLegacy("Chozo Statues", delegate (GenerationProgress progress)
				{
					progress.Message = "Placing Chozo Statues";
					for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05); i++)
					{
						float num2 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05));
						bool flag = false;
						int num3 = 0;
						int rand = Main.rand.Next(10);
						ushort item = (ushort)mod.TileType("MorphBallTile");
						if (rand == 1 || rand == 2)
						{
							item = (ushort)mod.TileType("ChargeBeamTile");
						}
						else if (rand == 3 || rand == 4)
						{
							item = (ushort)mod.TileType("IceBeamTile");
						}
						else if (rand == 5 || rand == 6)
						{
							item = (ushort)mod.TileType("WaveBeamTile");
						}
						else if (rand == 7)
						{
							item = (ushort)mod.TileType("SpazerTile");
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
			}
		}
		public static bool AddExpansion(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			int k = j;
			while (k < Main.maxTilesY)
			{
				if (Main.tile[i, k].active() && Main.tileSolid[(int)Main.tile[i, j].type])
				{
					int num = k - 1;
					if (Main.tile[i, num - 1].lava())
					{
						return false;
					}
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
					for (int wx = i - 2; wx < i + 3; wx++)
					{
						for (int wy = k - 4; wy < k; wy++)
						{
							WorldGen.PlaceWall(wx, wy, 5);
						}
					}
					for (int tx = i - 3; tx < i + 4; tx++)
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
					for (int ty = k - 4; ty < k; ty++)
					{
						Main.tile[i + 3, ty].slope(0);	
						Main.tile[i + 3, ty].halfBrick(false);
						Main.tile[i + 3, ty].active(true);	
						Main.tile[i + 3, ty].type = 38;
					}
					Main.tile[i - 3, k - 4].slope(0);	
					Main.tile[i - 3, k - 4].halfBrick(false);
					Main.tile[i - 3, k - 4].active(true);	
					Main.tile[i - 3, k - 4].type = 38;

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