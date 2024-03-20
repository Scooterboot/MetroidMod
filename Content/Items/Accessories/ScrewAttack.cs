using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	// legacy name because old suit addon system
	[LegacyName("ScrewAttackAddon")]
	[AutoloadEquip(EquipType.Front)]
	public class ScrewAttack : ModItem//HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Screw Attack");
			//Tooltip.SetDefault("Allows the user to double jump\n" + 
			/*"Allows somersaulting\n" + 
			"Damage enemies while someraulting\n" + 
			"Damage scales off of enemy's contact damage\n" +
			"Hold Left/Right and double jump to do a 'boost' ability");*/

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.noMelee = true;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.ScrewAttackTile>();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item), mp.screwAttackDmg);
		}
	}
}
