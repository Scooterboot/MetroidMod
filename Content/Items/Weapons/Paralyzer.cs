using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.Content.Projectiles.powerbeam;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Default;
using Terraria.Utilities;
using MetroidMod.Content.Projectiles.Paralyzer;
using MetroidMod.Content.Items.Miscellaneous;

namespace MetroidMod.Content.Items.Weapons
{
	public class Paralyzer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Paralyzer");
			Tooltip.SetDefault("'A gift of the Chozo'");

			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.width = 16;
			Item.height = 12;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.buyPrice(0, 50, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = Sounds.Items.Weapons.Paralyzer;
			Item.shoot = ModContent.ProjectileType<ParalyzerShot>();
			Item.shootSpeed = 16f;
			Item.crit = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteBar>(6)
				.AddIngredient<EnergyShard>(2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
