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
	public class ShaderTestItem : ModItem
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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<Projectiles.ShaderTestProj>();
			Item.knockBack = 4;
			Item.value = 12500;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shootSpeed = 4f;
		}
	}
}
