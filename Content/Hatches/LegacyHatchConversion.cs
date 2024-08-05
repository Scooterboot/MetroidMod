using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace MetroidMod.Content.Hatches
{
	internal class LegacyHatchConversion : ModSystem
	{
		// I don't think Main.tile is filled in OnWorldLoad, so we need to do this trick instead
		private bool done;
		private Dictionary<string, ModHatch> modHatchLookup;

		public override void SetStaticDefaults()
		{
			modHatchLookup = [];
			foreach(ModHatch modHatch in ModContent.GetContent<ModHatch>())
			{
				modHatchLookup[modHatch.Name] = modHatch;
			}
		}

		public override void PreUpdateWorld()
		{
			done = !(Main.mouseLeft && Main.mouseLeftRelease && Main.mouseRight && Main.mouseRightRelease);
			if (done) return;
			done = true;

			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int y = 0; y < Main.maxTilesY; y++)
				{
					Tile tile = Main.tile[x, y];
					if (!tile.HasTile) continue;

					ModTile modTile = ModContent.GetModTile(tile.TileType);
					if (modTile is not ModTile) continue;

					int hatchPos = modTile.Name.IndexOf("Hatch");
					if (hatchPos == -1) continue;

					var origin = TileUtils.GetTopLeftTileInMultitile(x, y);
					if (origin.X != x || origin.Y != y) continue;

					string hatchName = modTile.Name[..(hatchPos + "Hatch".Length)];
					
					bool open = modTile.Name.Contains("Open");
					bool vertical = modTile.Name.Contains("Vertical");

					int tileType = modHatchLookup[hatchName].GetTileType(open, vertical);

					HatchTilePlacement.PlaceHatchAt(tileType, x, y);
					Main.NewText($"{hatchName} {open} {vertical} {x} {y}");
				}
			}
		}
	}
}
