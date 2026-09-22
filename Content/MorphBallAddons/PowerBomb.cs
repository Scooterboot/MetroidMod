using System;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class PowerBomb : ModMorphBallSpecial, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombTile";

		public override string PowerBombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombProjectile";

		public override string PowerBombExplosionProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombExplosion";

		public override MorphBallAddonSlot AddonSlot => MorphBallAddonSlot.Special;

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return (Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
				? 4
				: 0
				;
		}
		public override void ItemSetDefaults()
		{
			Item.damage = 10;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.PowerBomb(player, PowerBombProjectile.ProjectileType, player.GetWeaponDamage(Item), Item);
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 15)
				.AddIngredient(ItemID.SoulofMight, 10)
				.AddIngredient(ItemID.SoulofFright, 10)
				.AddIngredient(ItemID.SoulofSight, 10)
				//.AddDecraftCondition(Condition.DownedMechBossAll)
				.Register();
		}
	}
}
