#region Using directives

using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Common.Systems
{
	public partial class MSystem : ModSystem
	{
		public static bool spawnedPhazonMeteor = false;

		public static void AddPhazon()
		{
			int lX = 200;
			int hX = Main.maxTilesX - 200;
			int lY = (int)Main.worldSurface;
			int hY = Main.maxTilesY - 200;

			int minSpread = 5;
			int maxSpread = 8;
			int minFrequency = 5;
			int maxFrequency = 8;
			WorldGen.OreRunner(WorldGen.genRand.Next(lX, hX), WorldGen.genRand.Next(lY, hY), WorldGen.genRand.Next(minSpread, maxSpread + 1), WorldGen.genRand.Next(minFrequency, maxFrequency + 1), (ushort)ModContent.TileType<Content.Tiles.PhazonTile>());
		}

		public static void DropPhazonMeteor()
		{
			bool generateSuccessful = false;

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			// Check to get a valid position for the generation of a Phazon Meteorite.
			// There's a certain solid tile treshold (starting at 600, stopping at 100, cancelling the generation), which needs to be met
			// Before the meteorite is allowed to generate.
			float solidTileTreshhold = 600f;
			while (!generateSuccessful)
			{
				float spawnMargin = Main.maxTilesX * 0.08f;
				int genY = (int)(Main.worldSurface * 0.3);
				int genX = Main.rand.Next(150, Main.maxTilesX - 150);

				// Do not allow a Phazon Meteorite to spawn too close to the center of the world (spawnpoint).
				while (Math.Abs(genX - Main.spawnTileX) < spawnMargin)
				{
					genX = Main.rand.Next(150, Main.maxTilesX - 150);
				}

				while (genY < Main.maxTilesY)
				{
					if (Main.tile[genX, genY].HasTile && Main.tileSolid[Main.tile[genX, genY].TileType] && !Main.tileSolidTop[Main.tile[genX, genY].TileType])
					{
						int genOffset = 15;
						int solidSpawnTiles = 0;

						for (int x = genX - genOffset; x < genX + genOffset; x++)
						{
							for (int y = genY - genOffset; y < genY + genOffset; y++)
							{
								if (WorldGen.SolidTile(x, y))
								{
									solidSpawnTiles++;
									if (Main.tile[x, y].TileType == TileID.Cloud || Main.tile[x, y].TileType == TileID.Sunplate)
									{
										solidSpawnTiles -= 100;
									}
								}
								else if (Main.tile[x, y].LiquidType > 0)
								{
									solidSpawnTiles--;
								}
							}
						}
						if (solidSpawnTiles < solidTileTreshhold)
						{
							solidTileTreshhold -= 0.5f;
							break;
						}

						if (TryGeneratePhazonMeteor(genX, genY))
						{
							generateSuccessful = true;
							break;
						}
					}
					genY++;
				}
				if (solidTileTreshhold < 100f)
				{
					return;
				}
			}

			if (generateSuccessful)
			{
				spawnedPhazonMeteor = true;
			}
		}

		public static bool TryGeneratePhazonMeteor(int genX, int genY)
		{
			if (genX < 50 || genX > Main.maxTilesX - 50 ||
				genY < 50 || genY > Main.maxTilesY - 50)
			{
				return false;
			}

			int spawnOffset = 35;
			Rectangle rectangle = new((genX - spawnOffset) * 16, (genY - spawnOffset) * 16, spawnOffset * 32, spawnOffset * 32);

			// If there's a player within the spawn area of the Phazon Meteorite, disallow spawning.
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				if (Main.player[i].active)
				{
					Rectangle playerRectangle = new(
						(int)(Main.player[i].position.X + (Main.player[i].width / 2) - (NPC.sWidth / 2) - NPC.safeRangeX),
						(int)(Main.player[i].position.Y + (Main.player[i].height / 2) - (NPC.sHeight / 2) - NPC.safeRangeY),
						NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2
					);

					if (rectangle.Intersects(playerRectangle))
					{
						return false;
					}
				}
			}

			// If there's an NPC within the spawn area of the Phazon Meteorite, disallow spawning.
			foreach (NPC who in Main.ActiveNPCs)
			{
				NPC npc = Main.npc[who.whoAmI];
				{
					if (rectangle.Intersects(npc.Hitbox))
					{
						return false;
					}
				}
			}

			// If there's a chest within the spawn area of the Phazon Meteorite, disallow spawning.
			for (int x = genX - spawnOffset; x < genX + spawnOffset; x++)
			{
				for (int y = genY - spawnOffset; y < genY + spawnOffset; y++)
				{
					if (Main.tile[x, y].HasTile && TileID.Sets.BasicChest[Main.tile[x, y].TileType])
					{
						return false;
					}
				}
			}

			// Initial spawn of Phazon Tiles, and removal of non-solid tiles.
			GeneratePhazonChunkAt(genX, genY, WorldGen.genRand.Next(17, 23), (x, y, margin) =>
			{
				if (y > genY + Main.rand.Next(-2, 3) - 5)
				{
					float distance = new Vector2(genX - x, genY - y).Length();
					if (distance < margin * 0.9f + Main.rand.Next(-4, 5))
					{
						if (!Main.tileSolid[Main.tile[x, y].TileType])
						{
							Main.tile[x, y].ClearTile();
						}
						Main.tile[x, y].TileType = (ushort)ModContent.TileType<Content.Tiles.PhazonTile>();
					}
				}
			});

			// Removal of tiles in the center of the 'crater'.
			GeneratePhazonChunkAt(genX, genY, WorldGen.genRand.Next(8, 14), (x, y, margin) =>
			{
				if (y > genY + Main.rand.Next(-2, 3) - 4)
				{
					float distance = new Vector2(genX - x, genY - y).Length();
					if (distance < margin * 0.8f + Main.rand.Next(-3, 4))
					{
						Main.tile[x, y].ClearTile();
					}
				}
			});

			// Placement of Phazon Core tiles.
			GeneratePhazonChunkAt(genX, genY + 4, WorldGen.genRand.Next(4, 6), (x, y, margin) =>
			{
				if (y > genY + Main.rand.Next(-2, 3) - 5 && (Math.Abs(genX - x) + Math.Abs(genY - y)) < margin * 1.5 + Main.rand.Next(-5, 5))
				{
					if (!Main.tileSolid[Main.tile[x, y].TileType])
					{
						Main.tile[x, y].ClearTile();
					}
					WorldGen.PlaceTile(x, y, ModContent.TileType<Content.Tiles.PhazonCore>(), true);
					WorldGen.SquareTileFrame(x, y);
				}
			});

			// First generation spread pass of Phazon Tiles.
			GeneratePhazonChunkAt(genX, genY, WorldGen.genRand.Next(25, 35), (x, y, margin) =>
			{
				float distance = new Vector2(genX - x, genY - y).Length();
				if (distance < margin * 0.7f)
				{
					if (Main.tile[x, y].TileType == TileID.Trees || Main.tile[x, y].TileType == TileID.CorruptThorns || Main.tile[x, y].TileType == TileID.CrimsonThorns)
					{
						WorldGen.KillTile(x, y, false, false, false);
					}
					Main.tile[x, y].Clear(Terraria.DataStructures.TileDataType.Liquid);//Main.tile[x, y].LiquidType = 0;
				}

				if (Main.tile[x, y].TileType == ModContent.TileType<Content.Tiles.PhazonTile>())
				{
					if (!WorldGen.SolidTile(x - 1, y) && !WorldGen.SolidTile(x + 1, y) && !WorldGen.SolidTile(x, y - 1) && !WorldGen.SolidTile(x, y + 1))
					{
						Main.tile[x, y].ClearTile();
					}
					else if ((Main.tile[x, y].IsHalfBlock || Main.tile[x - 1, y].TopSlope) && !WorldGen.SolidTile(x, y + 1))
					{
						Main.tile[x, y].ClearTile();
					}
				}

				WorldGen.SquareTileFrame(x, y);
				WorldGen.SquareWallFrame(x, y);
			});

			// Second generation spread pass of Phazon Tiles.
			GeneratePhazonChunkAt(genX, genY, WorldGen.genRand.Next(23, 32), (x, y, margin) =>
			{
				if (y > genY + WorldGen.genRand.Next(-3, 4) - 3 && Main.tile[x, y].HasTile && Main.rand.NextBool(10))
				{
					float distance = new Vector2(genX - x, genY - y).Length();
					if (distance < margin * 0.8f)
					{
						if (Main.tile[x, y].TileType == TileID.Trees || Main.tile[x, y].TileType == TileID.CorruptThorns || Main.tile[x, y].TileType == TileID.CrimsonThorns)
						{
							WorldGen.KillTile(x, y, false, false, false);
						}
						Main.tile[x, y].TileType = (ushort)ModContent.TileType<Content.Tiles.PhazonTile>();
						WorldGen.SquareTileFrame(x, y);
					}
				}
			});

			// Third generation spread pass of Phazon Tiles.
			GeneratePhazonChunkAt(genX, genY, WorldGen.genRand.Next(30, 38), (x, y, margin) =>
			{
				if (y > genY + WorldGen.genRand.Next(-2, 3) && Main.tile[x, y].HasTile && Main.rand.NextBool(20))
				{
					float distance = new Vector2(genX - x, genY - y).Length();
					if (distance < margin * 0.85f)
					{
						if (Main.tile[x, y].TileType == TileID.Trees || Main.tile[x, y].TileType == TileID.CorruptThorns || Main.tile[x, y].TileType == TileID.CrimsonThorns)
						{
							WorldGen.KillTile(x, y, false, false, false);
						}
						Main.tile[x, y].TileType = (ushort)ModContent.TileType<Content.Tiles.PhazonTile>();
						WorldGen.SquareTileFrame(x, y);
					}
				}
			});

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.WorldEvents.PhazonMeteor"), new Color(50, 255, 130));
			}
			else if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey($"Mods.{nameof(MetroidMod)}.WorldEvents.PhazonMeteor"), new Color(50, 255, 130), -1);
			}

			// Since we are not able to get here if we're in a multiplayer client session, removed the check.
			NetMessage.SendTileSquare(-1, genX, genY, 40, TileChangeType.None);

			return (true);
		}

		/// <summary>
		/// A helper method to spawn clusters of tiles for the Phazon Meteorite.
		/// </summary>
		/// <param name="genX">The X tile coordinate of the Phazon Meteorite spawn.</param>
		/// <param name="genY">The Y tile coordinate of the Phazon Meteorite spawn.</param>
		/// <param name="margin">The margin which is used to spread out tiles to a certain point around the spawn coordinates.</param>
		/// <param name="generationAction">The action to take for each tile within the chunk/spawn area.</param>
		private static void GeneratePhazonChunkAt(int genX, int genY, int margin, Action<int, int, int> generationAction)
		{
			for (int x = genX - margin; x < genX + margin; ++x)
			{
				for (int y = genY - margin; y < genY + margin; ++y)
				{
					generationAction(x, y, margin);
				}
			}
		}
	}
}
