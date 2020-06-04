using Terraria;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.accessories
{
	public class ScrewAttack : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack");
			Tooltip.SetDefault("Allows the user to double jump\n" + 
			"Allows somersaulting\n" + 
			"Damage enemies while someraulting\n" + 
			"Damage scales off of enemy's contact damage\n" +
			"Hold Left/Right and double jump to do a 'boost' ability");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 100;
			item.noMelee = true;
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
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(item),mp.screwAttackDmg);
		}
	}
}