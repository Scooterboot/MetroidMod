using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class KraidBag : ModItem
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
		public override int BossBagNPC => mod.NPCType("Kraid_Head");

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("KraidTissue"), Main.rand.Next(35, 66));
			player.QuickSpawnItem(mod.ItemType("UnknownPlasmaBeam"));
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("KraidTrophy"));
			}
			if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("KraidMask"));
			}
			if (Main.rand.Next(2) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("KraidPhantoonMusicBox"));
			}
		}

	}
}

