using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.SuitAddons
{
	public class ScrewAttack : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ScrewAttack/ScrewAttackItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ScrewAttack/ScrewAttackTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack");
			Tooltip.SetDefault("Allows the user to double jump\n" +
				"Allows somersaulting\n" +
				"Damage enemies while someraulting\n" +
				"Damage scales off of enemy's contact damage\n" +
				"Hold Left/Right and double jump to do a 'boost' ability");
			AddonSlot = SuitAddonSlotID.Misc_Attack;
		}
		public override void SetItemDefaults(Item item)
		{
			item.noMelee = true;
			item.DamageType = ModContent.GetInstance<DamageClasses.HunterDamageClass>();
			item.damage = 100;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Accessories.ScrewAttack>()
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item), mp.screwAttackDmg);
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			mp.screwAttack = true;
			mp.screwAttackDmg = Math.Max(player.GetWeaponDamage(Item), mp.screwAttackDmg);
		}
	}
}
