using System;
using System.Collections.Generic;
using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Biomes;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MetroidMod.Content.Subworlds
{
	public class MetroidDeepnest : Subworld
	{
		public override int Width => Main.maxTilesX / 2;

		public override int Height => Main.maxTilesY;

		public override WorldGenConfiguration Config => WorldGenConfiguration.FromEmbeddedPath("Terraria.GameContent.WorldBuilding.Configuration.json");


		// set to false for a "temporary" generation
		// temporarily set to false for worldgen-dev purposes, TODO set to true
		public override bool ShouldSave => false;

		// set to true to revert any changes to the player inventory and whatnot when exiting the subworld
		public override bool NoPlayerSaving => false;

		public override void CopyMainWorldData()
		{
			SubworldSystem.CopyWorldData("!" + nameof(MSystem.MetroidGenVars.metroidHiveLocations), MSystem.MetroidGenVars.metroidHiveLocations);
		}

		public override void ReadCopiedMainWorldData()
		{
			MSystem.MetroidGenVars.metroidHiveLocations = SubworldSystem.ReadCopiedWorldData<List<Point>>("!" + nameof(MSystem.MetroidGenVars.metroidHiveLocations));

			foreach (Point p in MSystem.MetroidGenVars.metroidHiveLocations)
			{
				MetroidMod.Instance.Logger.Info($"Received tile data: {p}");
			}
		}



		public override void Update()
		{
			SubworldSystem.hideUnderworld = true;

			// Update liquids!!!
			if (++Liquid.skipCount > 1)
			{
				Liquid.UpdateLiquid();
				Liquid.skipCount = 0;
			}
		}

		public override void DrawMenu(GameTime gameTime)
		{
			// TODO: Add file
			// if (Mod.RequestAssetIfExists<Texture2D>($"Assets/Textures/Backgrounds/DeepnestLoadingScreen", out var asset))
			// {
			// 	Main.spriteBatch.Draw(asset.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			// }
			Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Main.statusText, new Vector2(Main.screenWidth, Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2f, Color.White);
		}

		public override List<GenPass> Tasks => new()
		{
			new ResetPass(),
			WorldGen.VanillaGenPasses["Reset"],
			new TerrainPass(),
			WorldGen.VanillaGenPasses["Dunes"],
			WorldGen.VanillaGenPasses["Sand Patches"],
			WorldGen.VanillaGenPasses["Tunnels"],
			WorldGen.VanillaGenPasses["Mount Caves"],
			WorldGen.VanillaGenPasses["Dirt Wall Backgrounds"],
			WorldGen.VanillaGenPasses["Rocks In Dirt"],
			WorldGen.VanillaGenPasses["Dirt In Rocks"],
			WorldGen.VanillaGenPasses["Clay"],
			WorldGen.VanillaGenPasses["Small Holes"],
			WorldGen.VanillaGenPasses["Dirt Layer Caves"],
			WorldGen.VanillaGenPasses["Rock Layer Caves"],
			WorldGen.VanillaGenPasses["Surface Caves"],
			WorldGen.VanillaGenPasses["Wavy Caves"],
			WorldGen.VanillaGenPasses["Grass"],
			WorldGen.VanillaGenPasses["Dirt To Mud"],
			WorldGen.VanillaGenPasses["Silt"],
			WorldGen.VanillaGenPasses["Shinies"],
			WorldGen.VanillaGenPasses["Webs"],
			WorldGen.VanillaGenPasses["Underworld"],
			WorldGen.VanillaGenPasses["Lakes"],
			WorldGen.VanillaGenPasses["Mountain Caves"],
			WorldGen.VanillaGenPasses["Gems"],
			WorldGen.VanillaGenPasses["Gravitating Sand"],
			WorldGen.VanillaGenPasses["Dirt Rock Wall Runner"],
			WorldGen.VanillaGenPasses["Altars"],
			WorldGen.VanillaGenPasses["Settle Liquids"],
			WorldGen.VanillaGenPasses["Remove Water From Sand"],
			WorldGen.VanillaGenPasses["Smooth World"],
			WorldGen.VanillaGenPasses["Waterfalls"],
			WorldGen.VanillaGenPasses["Wall Variety"],
			WorldGen.VanillaGenPasses["Life Crystals"],
			WorldGen.VanillaGenPasses["Statues"],
			//WorldGen.VanillaGenPasses["Buried Chests"], // Breaks because we have no desert. TODO: Fix
			WorldGen.VanillaGenPasses["Surface Chests"],
			WorldGen.VanillaGenPasses["Gem Caves"],
			WorldGen.VanillaGenPasses["Cave Walls"],
			WorldGen.VanillaGenPasses["Quick Cleanup"],
			WorldGen.VanillaGenPasses["Pots"],
			WorldGen.VanillaGenPasses["Spreading Grass"],
			WorldGen.VanillaGenPasses["Traps"],
			WorldGen.VanillaGenPasses["Piles"],
			WorldGen.VanillaGenPasses["Spawn Point"],
			WorldGen.VanillaGenPasses["Grass Wall"],
			WorldGen.VanillaGenPasses["Sunflowers"],
			WorldGen.VanillaGenPasses["Planting Trees"],
			WorldGen.VanillaGenPasses["Herbs"],
			WorldGen.VanillaGenPasses["Dye Plants"],
			WorldGen.VanillaGenPasses["Weeds"],
			WorldGen.VanillaGenPasses["Vines"],
			WorldGen.VanillaGenPasses["Flowers"],
			WorldGen.VanillaGenPasses["Mushrooms"],
			WorldGen.VanillaGenPasses["Random Gems"],
			WorldGen.VanillaGenPasses["Tile Cleanup"],
			WorldGen.VanillaGenPasses["Stalac"],
			WorldGen.VanillaGenPasses["Remove Broken Traps"],

			// This is the order because we don't want the
			// passages to generate over the Lab
			// nor over the Hives themselves -Armi
			new HivePassagesPass(),
			new HivesPass(),
			new LabPass(),
			WorldGen.VanillaGenPasses["Settle Liquids Again"],
			//WorldGen.VanillaGenPasses["Final Cleanup"], // this broke, not sure why. - Armi
			new FinishPass()
		};

		internal class ResetPass : GenPass
		{
			public ResetPass() : base("Metroid Deepnest: Reset Generation Variables", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				MSystem.MetroidGenVars.metroidHiveThicknessValues = new();

				// do worldgen business here
				WorldGen.generatingWorld = true;
				GenVars.configuration = ModContent.GetInstance<MetroidDeepnest>().Config;
				GenVars.structures = new StructureMap();

				// TODO: Some of this is unnecessary. Comb.
				GenVars.desertHiveHigh = Main.maxTilesY;
				GenVars.desertHiveLow = 0;
				GenVars.desertHiveLeft = Main.maxTilesX;
				GenVars.desertHiveRight = 0;
				GenVars.worldSurfaceLow = 0.0;
				GenVars.worldSurface = 0.0;
				GenVars.worldSurfaceHigh = 0.0;
				GenVars.rockLayerLow = 0.0;
				GenVars.rockLayer = 0.0;
				GenVars.rockLayerHigh = 0.0;
				GenVars.copper = 7;
				GenVars.iron = 6;
				GenVars.silver = 9;
				GenVars.gold = 8;
				GenVars.dungeonSide = 0;
				GenVars.jungleHut = (ushort)WorldGen.genRand.Next(5);
				GenVars.shellStartXLeft = 0;
				GenVars.shellStartYLeft = 0;
				GenVars.shellStartXRight = 0;
				GenVars.shellStartYRight = 0;
				GenVars.PyrX = null;
				GenVars.PyrY = null;
				GenVars.numPyr = 0;
				GenVars.jungleMinX = -1;
				GenVars.jungleMaxX = -1;
				GenVars.snowMinX = new int[Main.maxTilesY];
				GenVars.snowMaxX = new int[Main.maxTilesY];
				GenVars.snowTop = 0;
				GenVars.snowBottom = 0;
				GenVars.skyLakes = 1;
				if (Main.maxTilesX > 8000)
					GenVars.skyLakes++;

				if (Main.maxTilesX > 6000)
					GenVars.skyLakes++;

				GenVars.beachBordersWidth = 275;
				GenVars.beachSandRandomCenter = GenVars.beachBordersWidth + 5 + 40;
				GenVars.beachSandRandomWidthRange = 20;
				GenVars.beachSandDungeonExtraWidth = 40;
				GenVars.beachSandJungleExtraWidth = 20;
				GenVars.oceanWaterStartRandomMin = 220;
				GenVars.oceanWaterStartRandomMax = GenVars.oceanWaterStartRandomMin + 40;
				GenVars.oceanWaterForcedJungleLength = 275;
				GenVars.leftBeachEnd = 0;
				GenVars.rightBeachStart = 0;
				GenVars.evilBiomeBeachAvoidance = GenVars.beachSandRandomCenter + 60;
				GenVars.evilBiomeAvoidanceMidFixer = 50;
				GenVars.lakesBeachAvoidance = GenVars.beachSandRandomCenter + 20;
				GenVars.smallHolesBeachAvoidance = GenVars.beachSandRandomCenter + 20;
				GenVars.surfaceCavesBeachAvoidance = GenVars.beachSandRandomCenter + 20;
				GenVars.surfaceCavesBeachAvoidance2 = GenVars.beachSandRandomCenter + 20;
				GenVars.jungleOriginX = 0;
				GenVars.snowOriginLeft = 0;
				GenVars.snowOriginRight = 0;
				GenVars.logX = -1;
				GenVars.logY = -1;
				GenVars.dungeonLocation = 0;

				// Code so vanilla worldgen code doesn't crash out
				GenVars.shimmerPosition = new Relogic.Utilities.Vector2D(Main.maxTilesX / 2, Main.maxTilesY / 2);
			}
		}
		
		internal class HivePassagesPass : GenPass
		{
			public HivePassagesPass() : base("Metroid Deepnest: Hiving 1", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = "Generating Hive Tunnels";

				// Should this step be moved into its own GenPass? maybe into ResetPass? god -Armi
				// So first, we need to find the midpoint of all the hives.
				Point centerPos = MSystem.MetroidGenVars.labsPosition = MSystem.MetroidGenVars.metroidHiveLocations.CenterOfPoints();
				// Go down a little. We'll need a bit of room later on.
				centerPos.Y += 30;

				for (int i = 0; i < MSystem.MetroidGenVars.metroidHiveLocations.Count; i++)
				{
					Point hivePoint = MSystem.MetroidGenVars.metroidHiveLocations[i];
					MSystem.MetroidGenVars.metroidHiveThicknessValues.Add(WorldGen.genRand.Next(35, 50));
					int thickness = MSystem.MetroidGenVars.metroidHiveThicknessValues[i];
					MSystem.Line(new(centerPos.X / 2, centerPos.Y), new(hivePoint.X / 2, hivePoint.Y), thickness, (ushort)ModContent.TileType<Tiles.MetroidHive>(), (ushort)ModContent.WallType<Walls.MetroidHiveWallNatural>(), true, false, true);
				}
			}
		}
		
		internal class HivesPass : GenPass
		{
			public HivesPass() : base("Metroid Deepnest: Hiving 2", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = "Generating Hives";
				// TODO: Shape the hives.

				// Replicate positioning of the caves from the overworld and build
				foreach (Point pos in MSystem.MetroidGenVars.metroidHiveLocations)
				{
					// offset by 1 because the exit tile is slightly differently sized :(
					MSystem.MetroidHiveEntranceExitTile(pos.X / 2 - 1, pos.Y + 1, true);
				}
			}
		}

		internal class LabPass : GenPass
		{
			public LabPass() : base("Metroid Deepnest: The Laboratory", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = "Generating The Laboratory";

				Point centerPos = MSystem.MetroidGenVars.labsPosition;
				centerPos.X /= 2;

				// Half-Pipe Shape
				int thickness = 200;
				for (int x = (int)(centerPos.X - thickness / 2.0); (double)x < centerPos.X + thickness / 2.0; x++)
				{
					for (int y = (int)(centerPos.Y - thickness / 2.0); (double)y < centerPos.Y + thickness / 2.0; y++)
					{
						double funnyX = Math.Abs((double)x - centerPos.X);
						Tile tile = Main.tile[x, y];
						tile.LiquidAmount = 0;
						if (y < centerPos.Y)
						{
							if (funnyX > thickness * 0.4)
							{
								tile.HasTile = true;
								tile.TileType = (ushort)ModContent.TileType<Tiles.MetroidHive>();
							} 
							else
							{
								tile.HasTile = false;
							}
							if (funnyX < thickness * 0.5)
							{
								tile.WallType = WallID.None;
							}
						}
						else
						{
							double funnyY = Math.Abs((double)y - centerPos.Y);
							double distFromCenter = Math.Sqrt(funnyX * funnyX + funnyY * funnyY);
							if (distFromCenter < thickness * 0.4)
							{
								tile.HasTile = false;
								tile.WallType = WallID.None;
								if (tile.LiquidAmount > 0)
								{
									tile.LiquidAmount = 0;
								}
							}
							else if (distFromCenter < thickness * 0.5)
							{
								tile.HasTile = true;
								tile.TileType = (ushort)ModContent.TileType<Tiles.MetroidHive>();
								if (distFromCenter > thickness * 0.6)
								{
									tile.WallType = (ushort)ModContent.WallType<Walls.MetroidHiveWallNatural>();
								}
								if (tile.LiquidAmount > 0)
								{
									tile.LiquidAmount = 0;
								}
							}
						}
					}
				}

				// Ok, now we make the holes so the area can be accessed.
				centerPos.Y += 30;

				for (int i = 0; i < MSystem.MetroidGenVars.metroidHiveLocations.Count; i++)
				{
					Point hivePoint = MSystem.MetroidGenVars.metroidHiveLocations[i];
					double angle = Math.Atan2((hivePoint.Y - centerPos.Y), (hivePoint.X / 2 - centerPos.X));

					double soonX = thickness * Math.Cos(angle) * 4 / 10;
					double soonY = thickness * Math.Sin(angle) * 4 / 10;
					if (centerPos.Y + soonY < centerPos.Y - 30)
					{
						soonX *= 1.3;
						soonY *= 1.3;
					}
					Point hivePointFromFunny = new((int)(centerPos.X + soonX), (int)(centerPos.Y + soonY));
					int thickness2 = MSystem.MetroidGenVars.metroidHiveThicknessValues[i];
					MSystem.Line(new(centerPos.X, centerPos.Y), hivePointFromFunny, thickness2, TileID.Dirt, WallID.None, true, true, false);
				}
			}
		}

		internal class FinishPass : GenPass
		{
			public FinishPass() : base("Metroid Deepnest: Finish", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				WorldGen.generatingWorld = false;
			}
		}
	}
}
