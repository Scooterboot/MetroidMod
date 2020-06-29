using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class TorizoBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;
			item.expert = true;
			item.rare = -12;
		}

		public override bool CanRightClick() => true;
		public override int BossBagNPC => mod.NPCType("Torizo");

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("EnergyShard"), Main.rand.Next(25, 51));
			player.QuickSpawnItem(mod.ItemType("ChoziteOre"), Main.rand.Next(30, 90));
			if (Main.rand.Next(2) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("TorizoMusicBox"));
			}
			if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("TorizoMask"));
			}
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("TorizoTrophy"));
			}				
		}
	}
}

