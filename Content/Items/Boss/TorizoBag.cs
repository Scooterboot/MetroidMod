using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Boss
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
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.expert = true;
			Item.rare = -12;
		}

		public override bool CanRightClick() => true;
		public override int BossBagNPC => ModContent.NPCType<NPCs.Torizo.Torizo>();

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Miscellaneous.EnergyShard>(), Main.rand.Next(25, 51));
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.ChoziteOre>(), Main.rand.Next(30, 90));
			if (Main.rand.NextBool(2))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.TorizoMusicBox>());
			}
			if (Main.rand.NextBool(3))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Vanity.TorizoMask>());
			}
			if (Main.rand.NextBool(5))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.TorizoTrophy>());
			}				
		}
	}
}

