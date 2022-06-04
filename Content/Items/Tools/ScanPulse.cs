using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidModPorted.Common.Systems;

namespace MetroidModPorted.Content.Items.Tools
{
	public class ScanPulse : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scan Pulse");
			Tooltip.SetDefault("Reveals the map around you");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 30;
			Item.height = 28;
			Item.UseSound = SoundID.Item15;
			Item.useStyle = 4;
			Item.useTime = 120;
			Item.useAnimation = 120;
			Item.autoReuse = true;
			Item.value = 50000;
			Item.rare = -12;
			Item.expert = true;
		}

		public override bool? UseItem(Player player)
		{
			Point center = player.Center.ToTileCoordinates();
			Reveal(center.X, center.Y);
			return true;
		}

		public static void Reveal(int x, int y)	
		{
			for (int i = x - 100; i < x + 100; i++)
			{
				for (int j = y - 100; j < y + 100; j++)
				{
					if (WorldGen.InWorld(i, j))
					{
						MSystem.hit[i, j] = true;
						if (Main.tileSpelunker[Main.tile[i, j].TileType])
						{
							Main.Map.UpdateLighting(i, j, 200);
						}
						else
						{
							Main.Map.UpdateLighting(i, j, 80);
						}
					}
				}
			}
			Main.refreshMap = true;
		}
	}
}
