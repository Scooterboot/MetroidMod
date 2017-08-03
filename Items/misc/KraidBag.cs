using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
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
			bossBagNPC = mod.NPCType("Kraid_Head");
		}
public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			
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

