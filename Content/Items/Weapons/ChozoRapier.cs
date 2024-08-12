using System;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons
{
	public class ChozoRapier : ModItem
	{
		private readonly int defUseTime = 10;

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = (int)(Common.Configs.MConfigItems.Instance.damageChoziteShortsword * 1.5);
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = Item.useAnimation = defUseTime;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.shoot = ModContent.ProjectileType<Projectiles.ChozoRapierStab>();
			Item.knockBack = 4;
			Item.value = 12500;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shootSpeed = 1f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = defUseTime * 3;
				if (player.itemTime == 0)
				{
					return true;
				}
				return false;
			}
			Item.useTime = defUseTime;
			return true;
		}
		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Swing;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Rapier;
			}
		}
		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				int dir = Math.Sign(Main.MouseWorld.X - player.Center.X);
				if (player.velocity.Y == 0)
				{
					player.velocity.Y = -4f * player.gravDir;
					player.velocity.X = -7f * dir;
				}
				else
				{
					player.velocity.X = -5.5f * dir;
				}
				return true;
			}
			return base.UseItem(player);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.ChozoRapierSlash>(), damage, knockback);
				SoundEngine.PlaySound(Sounds.NPCs.TorizoWave, position);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.EnergyShard>(8)
				.AddIngredient<ChoziteShortsword>()
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
