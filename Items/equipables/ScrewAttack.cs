using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Items.equipables
{
	public class ScrewAttack : ModItem
	{
		bool screwAttack = false;
		int screwAttackSpeed = 0;
		int screwSpeedDelay = 0;
		int proj = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack");
			Tooltip.SetDefault("Allows the user to double jump\n" + 
			"Allows somersaulting\n" + 
			"Damage enemies while someraulting\n" + 
			"Hold Left/Right and double jump to do a 'boost' ability");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 40000;
			item.rare = 7;
			item.accessory = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ScrewAttackTile");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.spaceJumpBoots = true;
			mp.screwAttack = 1;
		}
		public override bool CanEquipAccessory(Player player, int slot)
		{
		    for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
		    {
				if(k != slot && (player.armor[k].type == mod.ItemType("ScrewSpaceBooster") || player.armor[k].type == mod.ItemType("TerraBooster")))
				{
					return false;
				}
		    }
		    return true;
		}
	}
}