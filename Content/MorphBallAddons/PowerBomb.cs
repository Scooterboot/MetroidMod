using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class PowerBomb : ModMBSpecial
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombProjectile";

		public override string ExplosionTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombExplosion";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb");
			Tooltip.SetDefault("-Press the Power Bomb Key to set off a Power Bomb (20 second cooldown)\n" +
			"-Power Bombs create large explosions that vacuum in items afterwards\n" +
			"-Power Bombs ignore 50% of enemy defense and can deal ~1400 damage total");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 25;
			item.noMelee = true;
			item.value = Terraria.Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.PowerBomb(player, ProjectileType, player.GetWeaponDamage(Item.Item), Item.Item);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 15)
				.AddIngredient(ItemID.SoulofMight, 10)
				.AddIngredient(ItemID.SoulofFright, 10)
				.AddIngredient(ItemID.SoulofSight, 10)
				.Register();
		}
	}
}
