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
		public override int Width => Main.maxTilesX;

		public override int Height => Main.maxTilesY;

		// set to false for a "temporary" generation
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
			new BasicPass()
		};

		// Very basic GenPass. Because why have a void when you can suffocate in dirt? - Armipotent
		internal class BasicPass : GenPass
		{
			public BasicPass() : base("Metroid Deepnest: Basic Pass", 1) { }

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				// do worldgen business here
				WorldGen.generatingWorld = true;
				Main.worldSurface = Main.maxTilesY / 7;
				progress.Message = "Generating terrain";
				for (int x = 0; x < Main.maxTilesX; x++)
				{
					for (int y = Main.maxTilesY / 8; y < Main.maxTilesY; y++)
					{
						progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
						Tile tile = Main.tile[x, y];
						tile.HasTile = true;
						tile.TileType = (ushort)ModContent.TileType<Tiles.MetroidHive>();
						tile.WallType = (ushort)ModContent.WallType<Walls.MetroidHiveWallNatural>();
					}
				}
				WorldGen.generatingWorld = false;
			}
		}
	}
}
