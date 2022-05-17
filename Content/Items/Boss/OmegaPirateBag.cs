using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Boss
{
	public class OmegaPirateBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.expert = true;
			Item.rare = ItemRarityID.Expert;
		}

		public override bool CanRightClick() => true;
		public override int BossBagNPC => ModContent.NPCType<NPCs.OmegaPirate.OmegaPirate>();

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Miscellaneous.PurePhazon>(), Main.rand.Next(30, 41));
			if (Main.rand.NextBool(2))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.OmegaPirateMusicBox>());
			}
			/*if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<OmegaPirateMask>());
			}
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<OmegaPirateTrophy>());
			}*/
		}
	}
}
