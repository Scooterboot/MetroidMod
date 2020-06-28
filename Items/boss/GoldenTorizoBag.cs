using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class GoldenTorizoBag : ModItem
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
		public override int BossBagNPC => mod.NPCType("GoldenTorizo");

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("ScrewAttack"));
			if (Main.rand.Next(2) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("TorizoMusicBox"));
			}
			/*if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("GoldenTorizoMask"));
			}
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("GoldenTorizoTrophy"));
			}*/
		}
	}
}

