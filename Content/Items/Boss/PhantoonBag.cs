using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Boss
{
	public class PhantoonBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
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
		public override int BossBagNPC => ModContent.NPCType<NPCs.Phantoon.Phantoon>();

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Miscellaneous.GravityGel>(), Main.rand.Next(35, 66));
			if (Main.rand.NextBool(5))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.PhantoonTrophy>());
			}
			if (Main.rand.NextBool(3))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Vanity.PhantoonMask>());
			}
			if (Main.rand.NextBool(2))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.KraidPhantoonMusicBox>());
			}
		}
	}
}

