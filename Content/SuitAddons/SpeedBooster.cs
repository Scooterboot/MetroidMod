using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.SuitAddons
{
	public class SpeedBooster : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpeedBooster/SpeedBoosterItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpeedBooster/SpeedBoosterTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
			Tooltip.SetDefault("Allows the user to run insanely fast\n" +
				"Damages enemies while running\n" +
				"Damage scales off of enemy's contact damage\n" +
				"While active, press DOWN to charge a Shine Spark\n" +
				"Then press JUMP to activate the charge");
			AddonSlot = SuitAddonSlotID.Boots_Speed;
		}
		public override void SetItemDefaults(Item item)
		{
			item.noMelee = true;
			item.DamageType = ModContent.GetInstance<DamageClasses.HunterDamageClass>();
			item.damage = 50;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.SerrisCoreX>()
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.JungleSpores, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item), mp.speedBoostDmg);
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.speedBooster = true;
			mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item), mp.speedBoostDmg);
		}
	}
}
