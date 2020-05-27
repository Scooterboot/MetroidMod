using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class OmegaPirateBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
			ItemID.Sets.ItemNoGravity[item.type] = true;
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
		public override int BossBagNPC => mod.NPCType("OmegaPirate");

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("PurePhazon"), Main.rand.Next(30, 41));
			/*if (Main.rand.Next(2) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("OmegaPirateMusicBox"));
			}
			if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("OmegaPirateMask"));
			}
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("OmegaPirateTrophy"));
			}*/
		}
	}
}

