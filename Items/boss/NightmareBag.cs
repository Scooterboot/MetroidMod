using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace MetroidMod.Items.boss
{
	public class NightmareBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 8));
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
		public override int BossBagNPC => mod.NPCType("Nightmare");

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("NightmareCoreX"));
			player.QuickSpawnItem(mod.ItemType("NightmareCoreXFragment"), Main.rand.Next(15, 25));
			if (Main.rand.Next(2) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("NightmareMusicBox"));
			}
			/*if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("NightmareMask"));
			}
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("NightmareTrophy"));
			}*/
		}
	}
}

