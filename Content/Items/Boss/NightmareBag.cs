using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace MetroidMod.Content.Items.Boss
{
	public class NightmareBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));

			ItemID.Sets.BossBag[Type] = true;
			SacrificeTotal = 3;
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
		public override int BossBagNPC => ModContent.NPCType<NPCs.Nightmare.Nightmare>();

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Miscellaneous.NightmareCoreX>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Miscellaneous.NightmareCoreXFragment>(), Main.rand.Next(15, 25));
			if (Main.rand.NextBool(2))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.NightmareMusicBox>());
			}
			/*if (Main.rand.NextBool(3))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Vanity.NightmareMask>());
			}
			if (Main.rand.NextBool(5))
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Tiles.NightmareTrophy>());
			}*/
		}
	}
}

