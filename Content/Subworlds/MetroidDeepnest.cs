using System.Collections.Generic;
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

		// set to false for a "temporary" generation
		// temporarily set to false for worldgen-dev purposes, TODO set to true
		public override bool ShouldSave => false;

		// set to true to revert any changes to the player inventory and whatnot when exiting the subworld
		public override bool NoPlayerSaving => false;

		public static class CustomGenVars
		{
			public static int thisIsATemplateName = 50;
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
			new FillInPass(),
			new LabPass(),
			new FinishPass()
		};

		internal class ResetPass : GenPass
		{
			public ResetPass() : base("Metroid Deepnest: Reset Generation Variables", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				// do worldgen business here
				WorldGen.generatingWorld = true;
				Main.worldSurface = Main.maxTilesY / 7;
				Main.rockLayer = Main.maxTilesY * 2 / 7;
			}
		}

		internal class FillInPass : GenPass
		{
			public FillInPass() : base("Metroid Deepnest: Fill-In Pass", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				// do worldgen business here
				progress.Message = "Generating terrain";
				for (int x = 0; x < Main.maxTilesX; x++)
				{
					for (int y = Main.maxTilesY / 8; y < (int)Main.worldSurface; y++)
					{
						progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
						Tile tile = Main.tile[x, y];
						tile.HasTile = true;
						tile.TileType = TileID.Dirt;
					}
					for (int y = (int)Main.worldSurface; y < Main.rockLayer; y++)
					{
						progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
						Tile tile = Main.tile[x, y];
						tile.HasTile = true;
						tile.TileType = TileID.Stone;
					}
					for (int y = (int)Main.rockLayer; y < Main.maxTilesY; y++)
					{
						progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
						Tile tile = Main.tile[x, y];
						tile.HasTile = true;
						tile.TileType = (ushort)ModContent.TileType<Tiles.MetroidHive>();
						tile.WallType = (ushort)ModContent.WallType<Walls.MetroidHiveWallNatural>();
					}
				}
			}
		}
		
		internal class CavesPass : GenPass
		{
			public CavesPass() : base("Metroid Deepnest: Caving", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = "Generating Caves";

				// Replicate positioning of the caves from the overworld and build
			}
		}

		internal class LabPass : GenPass
		{
			public LabPass() : base("Metroid Deepnest: The Laboratory", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = "Generating The Laboratory";

				// LABS CODE HERE
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
