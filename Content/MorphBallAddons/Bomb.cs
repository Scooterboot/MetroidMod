using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class Bomb : ModMorphBallBomb
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Bomb/BombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Bomb/BombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Bomb/BombProjectile";

		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.bombDamage = player.GetWeaponDamage(Item);
			mp.Bomb(player, BombProjectile.ProjectileType, Item);
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 13;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Green;
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(2)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(3)
				.AddIngredient(ItemID.Bomb, 15)
				.AddIngredient(ItemID.ManaCrystal, 1)
				.AddDecraftCondition(new Condition("ded birb", () => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo)))
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
