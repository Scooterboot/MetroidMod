using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
	public class SerrisBag : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Treasure Bag";
			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;
			item.toolTip = "Right click to open";
			item.expert = true;
			item.rare = -12;
			bossBagNPC = mod.NPCType("Serris_Head");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override DrawAnimation GetAnimation()
		{
			return new DrawAnimationVertical(5, 8);
		}
public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("SerrisCoreX"));
			if (Main.rand.Next(5) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("SerrisTrophy"));
			}
			if (Main.rand.Next(3) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("SerrisMask"));
			}
			if (Main.rand.Next(2) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("SerrisMusicBox"));
			}
		}

	}
}

