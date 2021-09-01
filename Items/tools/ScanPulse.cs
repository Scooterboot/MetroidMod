using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Worlds;

namespace MetroidMod.Items.tools
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
			item.maxStack = 1;
			item.width = 30;
			item.height = 28;
			item.UseSound = SoundID.Item15;
			item.useStyle = 4;
			item.useTime = 120;
			item.useAnimation = 120;
			item.autoReuse = true;
			item.value = 50000;
			item.rare = -12;
			item.expert = true;
		}

		public override bool UseItem(Player player)
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
                        MWorld.hit[i, j] = true;
						if (Main.tileSpelunker[Main.tile[i, j].type])
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